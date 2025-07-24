using MechaSoft.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using System.Security.Claims;


namespace MechaSoft.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditableEntityInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext? context)
    {
        if (context == null) return;

        var currentUser = GetCurrentUser();
        var utcNow = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is IAuditable auditableEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditableEntity.CreatedAt = utcNow;
                        auditableEntity.CreatedBy = currentUser;
                        auditableEntity.UpdatedAt = utcNow;
                        auditableEntity.UpdatedBy = currentUser;
                        auditableEntity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        var isDeletedProperty = entry.Property(nameof(IAuditable.IsDeleted));
                        if (isDeletedProperty.IsModified && (bool)isDeletedProperty.CurrentValue!)
                        {
                            // É um soft delete
                            auditableEntity.UpdatedAt = utcNow;
                            auditableEntity.UpdatedBy = currentUser;
                        }
                        else if (!auditableEntity.IsDeleted)
                        {
                            // Modificação normal
                            auditableEntity.UpdatedAt = utcNow;
                            auditableEntity.UpdatedBy = currentUser;
                        }

                        // Prevenir modificação dos campos de criação
                        entry.Property(nameof(IAuditable.CreatedAt)).IsModified = false;
                        entry.Property(nameof(IAuditable.CreatedBy)).IsModified = false;
                        break;
                }
            }
        }
    }

    private string GetCurrentUser()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.User?.Identity?.IsAuthenticated == true)
        {
            return httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? httpContext.User.FindFirst(ClaimTypes.Name)?.Value
                   ?? httpContext.User.Identity.Name
                   ?? "System";
        }
        return "System";
    }
}
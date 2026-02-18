using MechaSoft.Data.Context;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.Application.Common.Behaviours;

internal class UnitOfWorkBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;

    public UnitOfWorkBehaviour(IUnitOfWork unitOfWork, ApplicationDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (IsNotCommand())
        {
            return await next();
        }

        // Com EnableRetryOnFailure, transações manuais têm de correr dentro da execution strategy
        // para que toda a unidade (begin + handler + commit) seja retentável.
        var strategy = _context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await next();
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        });
    }

    private static bool IsNotCommand()
    {
        return !typeof(TRequest).Name.EndsWith("Command");
    }
}


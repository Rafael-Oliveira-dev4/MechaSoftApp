using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;

namespace MechaSoft.Application.CQ.Accounts.Queries.SuggestUsername;

public class SuggestUsernameQueryHandler : IRequestHandler<SuggestUsernameQuery, Result<SuggestUsernameResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public SuggestUsernameQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SuggestUsernameResponse>> Handle(
        SuggestUsernameQuery request,
        CancellationToken cancellationToken)
    {
        var baseUsername = request.Username;
        var suggestions = new List<string>();
        var currentYear = DateTime.Now.Year;

        // Gerar diferentes tipos de sugestões
        var possibleSuggestions = new List<string>
        {
            $"{baseUsername}{new Random().Next(100, 999)}",
            $"{baseUsername}{currentYear}",
            $"{baseUsername}{currentYear % 100}", // Ex: 2024 -> 24
            $"{baseUsername}_{new Random().Next(10, 99)}",
            $"{baseUsername}_user",
            $"_{baseUsername}",
            $"{baseUsername}_",
            $"{baseUsername}{new Random().Next(1, 99)}",
        };

        // Verificar disponibilidade de cada sugestão
        foreach (var suggestion in possibleSuggestions)
        {
            if (suggestions.Count >= 5) break; // Limitar a 5 sugestões
            
            var existingUser = await _unitOfWork.UserRepository.GetByUsernameAsync(suggestion);
            if (existingUser == null)
            {
                suggestions.Add(suggestion);
            }
        }

        var response = new SuggestUsernameResponse(suggestions);
        return Result<SuggestUsernameResponse>.Success(response);
    }
}


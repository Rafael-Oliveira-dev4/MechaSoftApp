using MechaSoft.Application.CQ.Accounts.Commands.RegisterUser;
using MechaSoft.Application.CQ.Accounts.Commands.LoginUser;
using MechaSoft.Application.CQ.Accounts.Commands.ChangePassword;
using MechaSoft.Application.CQ.Accounts.Commands.ResetPassword;
using MechaSoft.Application.CQ.Accounts.Commands.ForgotPassword;
using MechaSoft.Application.CQ.Accounts.Commands.RefreshToken;
using MechaSoft.Application.CQ.Accounts.Queries.GetUserProfile;
using MechaSoft.Application.CQ.Accounts.Queries.GetUsers;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MechaSoft.WebAPI.Endpoints;

public static class AccountEndpoints
{
    public static void RegisterAccountEndpoints(this IEndpointRouteBuilder routes)
    {
        var accounts = routes.MapGroup("api/accounts");

        // POST /api/accounts/register - Registrar novo usuário
        accounts.MapPost("/register", Commands.RegisterUser)
                .WithName("RegisterUser")
                .Produces<RegisterUserResponse>(201)
                .Produces<Error>(400);

        // POST /api/accounts/login - Fazer login
        accounts.MapPost("/login", Commands.LoginUser)
                .WithName("LoginUser")
                .Produces<LoginUserResponse>(200)
                .Produces<Error>(400);

        // PUT /api/accounts/change-password - Alterar senha
        accounts.MapPut("/change-password", Commands.ChangePassword)
                .WithName("ChangePassword")
                .Produces<ChangePasswordResponse>(200)
                .Produces<Error>(400);

        // POST /api/accounts/forgot-password - Solicitar reset de senha
        accounts.MapPost("/forgot-password", Commands.ForgotPassword)
                .WithName("ForgotPassword")
                .Produces<ForgotPasswordResponse>(200)
                .Produces<Error>(400);

        // POST /api/accounts/reset-password - Resetar senha
        accounts.MapPost("/reset-password", Commands.ResetPassword)
                .WithName("ResetPassword")
                .Produces<ResetPasswordResponse>(200)
                .Produces<Error>(400);

        // POST /api/accounts/refresh-token - Renovar tokens
        accounts.MapPost("/refresh-token", Commands.RefreshToken)
                .WithName("RefreshToken")
                .Produces<RefreshTokenResponse>(200)
                .Produces<Error>(400);

        // GET /api/accounts/profile/{userId} - Obter perfil do usuário
        accounts.MapGet("/profile/{userId:guid}", Queries.GetUserProfile)
                .WithName("GetUserProfile")
                .Produces<GetUserProfileResponse>(200)
                .Produces<Error>(404);

        // GET /api/accounts/users - Listar usuários com paginação
        accounts.MapGet("/users", Queries.GetUsers)
                .WithName("GetUsers")
                .Produces<GetUsersResponse>(200)
                .Produces<Error>(400);
    }

    private static class Commands
    {
        public static async Task<Results<CreatedAtRoute<RegisterUserResponse>, BadRequest<Error>>> RegisterUser(
            [FromServices] ISender sender,
            [FromBody] RegisterUserRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidInput);
            }

            var command = new RegisterUserCommand(
                request.Username,
                request.Email,
                request.Password,
                request.Role,
                request.CustomerId,
                request.EmployeeId
            );

            var result = await sender.Send(command);
            
            return result.IsSuccess 
                ? TypedResults.CreatedAtRoute(result.Value!, "GetUserProfile", new { userId = result.Value!.UserId })
                : TypedResults.BadRequest(result.Error!);
        }

        public static async Task<Results<Ok<LoginUserResponse>, BadRequest<Error>>> LoginUser(
            [FromServices] ISender sender,
            [FromBody] LoginUserRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidCredentials);
            }

            var command = new LoginUserCommand(request.Username, request.Password);
            var result = await sender.Send(command);
            
            return result.IsSuccess
                ? TypedResults.Ok(result.Value)
                : TypedResults.BadRequest(result.Error);
        }

        public static async Task<Results<Ok<ChangePasswordResponse>, BadRequest<Error>>> ChangePassword(
            [FromServices] ISender sender,
            [FromBody] ChangePasswordRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidInput);
            }

            var command = new ChangePasswordCommand(
                request.UserId,
                request.CurrentPassword,
                request.NewPassword
            );

            var result = await sender.Send(command);
            
            return result.IsSuccess
                ? TypedResults.Ok(result.Value)
                : TypedResults.BadRequest(result.Error);
        }

        public static async Task<Results<Ok<ForgotPasswordResponse>, BadRequest<Error>>> ForgotPassword(
            [FromServices] ISender sender,
            [FromBody] ForgotPasswordRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidInput);
            }

            var command = new ForgotPasswordCommand(request.Email);
            var result = await sender.Send(command);
            
            return result.IsSuccess
                ? TypedResults.Ok(result.Value)
                : TypedResults.BadRequest(result.Error);
        }

        public static async Task<Results<Ok<ResetPasswordResponse>, BadRequest<Error>>> ResetPassword(
            [FromServices] ISender sender,
            [FromBody] ResetPasswordRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidInput);
            }

            var command = new ResetPasswordCommand(
                request.Email,
                request.ResetToken,
                request.NewPassword,
                request.ConfirmNewPassword
            );

            var result = await sender.Send(command);
            
            return result.IsSuccess
                ? TypedResults.Ok(result.Value)
                : TypedResults.BadRequest(result.Error);
        }

        public static async Task<Results<Ok<RefreshTokenResponse>, BadRequest<Error>>> RefreshToken(
            [FromServices] ISender sender,
            [FromBody] RefreshTokenRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidToken);
            }

            var command = new RefreshTokenCommand(request.AccessToken, request.RefreshToken);
            var result = await sender.Send(command);
            
            return result.IsSuccess
                ? TypedResults.Ok(result.Value)
                : TypedResults.BadRequest(result.Error);
        }
    }

    private static class Queries
    {
        public static async Task<Results<Ok<GetUserProfileResponse>, NotFound<Error>>> GetUserProfile(
            [FromServices] ISender sender,
            Guid userId)
        {
            var query = new GetUserProfileQuery(userId);
            var result = await sender.Send(query);
            
            return result.IsSuccess
                ? TypedResults.Ok(result.Value)
                : TypedResults.NotFound(result.Error);
        }

        public static async Task<Results<Ok<GetUsersResponse>, BadRequest<Error>>> GetUsers(
            [FromServices] ISender sender,
            int pageNumber = 1,
            int pageSize = 10,
            UserRole? role = null,
            bool? isActive = null)
        {
            var query = new GetUsersQuery(pageNumber, pageSize, role, isActive);
            var result = await sender.Send(query);
            
            return result.IsSuccess
                ? TypedResults.Ok(result.Value)
                : TypedResults.BadRequest(result.Error);
        }
    }
}

// DTOs para Account Endpoints
public record RegisterUserRequest(
    string Username,
    string Email,
    string Password,
    UserRole Role,
    Guid? CustomerId = null,
    Guid? EmployeeId = null);

public record LoginUserRequest(
    string Username,
    string Password);

public record ChangePasswordRequest(
    Guid UserId,
    string CurrentPassword,
    string NewPassword);

public record ForgotPasswordRequest(
    string Email);

public record ResetPasswordRequest(
    string Email,
    string ResetToken,
    string NewPassword,
    string ConfirmNewPassword);

public record RefreshTokenRequest(
    string AccessToken,
    string RefreshToken);
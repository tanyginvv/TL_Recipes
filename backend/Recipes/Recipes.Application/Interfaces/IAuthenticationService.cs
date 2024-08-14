using Recipes.Application.Results;
using Recipes.Application.UseCases.Services.AuthService;

namespace Recipes.Application.Interfaces;

public interface IAuthenticationService
{
    Task<Result<AuthenticateUserDto>> AuthenticateUserAsync( string login, string password );
}
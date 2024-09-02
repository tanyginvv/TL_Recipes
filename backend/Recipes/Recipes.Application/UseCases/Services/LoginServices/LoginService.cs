using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Application.Results;
using Recipes.Application.PasswordHasher;

namespace Recipes.Application.UseCases.Services.LoginServices;

public class LoginService(
    IAuthTokenService authTokenService,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher )
    : ILoginService
{
    public async Task<Result<TokenDto>> LoginAsync( string login, string password )
    {
        User user = await userRepository.GetByLoginAsync( login );
        if ( user is null || !passwordHasher.VerifyPassword( password, user.PasswordHash ) )
        {
            return Result<TokenDto>.FromError( "Неверное имя пользователя или пароль" );
        }

        return await authTokenService.GenerateTokensAsync( user.Id );
    }
}
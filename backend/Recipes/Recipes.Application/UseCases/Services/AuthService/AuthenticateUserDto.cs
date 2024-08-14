namespace Recipes.Application.UseCases.Services.AuthService;

public class AuthenticateUserDto
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}
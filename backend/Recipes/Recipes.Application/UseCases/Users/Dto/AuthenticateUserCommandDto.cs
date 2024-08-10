namespace Recipes.Application.UseCases.Users.Dto;

public class AuthenticateUserCommandDto
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}
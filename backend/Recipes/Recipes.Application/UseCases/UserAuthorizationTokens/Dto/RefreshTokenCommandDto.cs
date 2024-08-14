namespace Recipes.Application.UseCases.UserAuthorizationTokens.Dto;

public class RefreshTokenCommandDto
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}
namespace Recipes.Application.UseCases.Services;

public class TokenDto
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}
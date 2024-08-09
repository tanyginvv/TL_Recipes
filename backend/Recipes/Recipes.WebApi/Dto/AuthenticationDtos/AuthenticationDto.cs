namespace Recipes.WebApi.Dto.AuthenticationDto;

public class AuthenticationDto
{
    public string Login { get; init; }
    public string PasswordHash { get; init; }
}
namespace Recipes.Application.UseCases.Users.Commands.AuthenticatePassword;

public class AuthenticateUserCommand
{
    public string Login { get; init; }
    public string PasswordHash { get; init; }
}
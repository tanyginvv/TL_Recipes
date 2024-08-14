namespace Recipes.Application.UseCases.Users.Commands;

public class CreateUserCommand
{
    public string Name { get; init; }
    public string Login { get; init; }
    public string Description { get; init; }
    public string PasswordHash { get; init; }
}
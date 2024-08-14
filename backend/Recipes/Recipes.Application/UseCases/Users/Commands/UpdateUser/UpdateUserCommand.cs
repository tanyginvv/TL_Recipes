namespace Recipes.Application.UseCases.Users.Commands.UpdateUser;

public class UpdateUserCommand
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string Login { get; init; }
    public string OldPasswordHash { get; init; }
    public string NewPasswordHash { get; init; }
}
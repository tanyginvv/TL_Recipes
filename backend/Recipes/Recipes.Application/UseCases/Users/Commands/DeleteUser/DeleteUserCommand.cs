namespace Recipes.Application.UseCases.Users.Commands.DeleteUser;

public class DeleteUserCommand
{
    public int userId { get; init; }
    public string PasswordHash { get; init; }
}

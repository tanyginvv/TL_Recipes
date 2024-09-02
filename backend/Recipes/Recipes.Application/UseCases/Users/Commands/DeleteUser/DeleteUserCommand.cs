namespace Recipes.Application.UseCases.Users.Commands.DeleteUser;

public class DeleteUserCommand
{
    public int UserId { get; init; }
    public string PasswordHash { get; init; }
}
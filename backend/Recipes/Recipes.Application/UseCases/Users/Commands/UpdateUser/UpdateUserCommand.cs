namespace Recipes.Application.UseCases.Users.Commands.UpdateUser;

public class UpdateUserCommand
{
    public int Id { get; set; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string Login { get; init; }
    public string OldPassword { get; init; }
    public string NewPassword { get; init; }
}
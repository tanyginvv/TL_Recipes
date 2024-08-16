namespace Recipes.Application.UseCases.Users.Dto;

public class UserDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string Login { get; init; }
    public int RecipesCount { get; init; }
}
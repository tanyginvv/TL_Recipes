namespace Recipes.Application.UseCases.Users.Dto;

public class GetUserLoginByIdQueryDto
{
    public int Id { get; init; }
    public string Login { get; init; }
}
namespace Recipes.Application.UseCases.Users.Dto
{
    public class GetUserByIdQueryDto
    {
        public int Id { get; init; }
        public string Login { get; init; }
    }
}

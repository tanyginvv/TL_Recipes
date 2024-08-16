
namespace Recipes.WebApi.Dto.UserDto;

public class RegistrateUserDto
{
    public string Name { get; init; }
    public string Login { get; init; }
    public string Description { get; init; }
    public string PasswordHash { get; init; }
}

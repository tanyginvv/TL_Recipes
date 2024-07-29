namespace Recipes.WebApi.Dto.UserDto
{
    public class RegistrateUserDto
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Recipes.WebApi.Dto.UserDtos;

public class RegisterUserDto
{
    [Required]
    [MaxLength( 50 )]
    public string Name { get; init; }

    [Required]
    [MaxLength(50)]
    public string Login { get; init; }

    [Required]
    public string Password { get; init; }
}
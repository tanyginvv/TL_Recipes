using System.ComponentModel.DataAnnotations;

namespace Recipes.WebApi.Dto.UserDtos;

public class LoginDto
{
    [Required]
    [MaxLength( 50 )]
    public string Login { get; init; }

    [Required]
    public string Password { get; init; }
}
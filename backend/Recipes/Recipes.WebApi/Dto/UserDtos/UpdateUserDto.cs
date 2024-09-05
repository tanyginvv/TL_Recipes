using System.ComponentModel.DataAnnotations;

namespace Recipes.WebApi.Dto.UserDtos;

public class UpdateUserDto
{
    [Required]
    [MaxLength( 50 )]
    public string Name { get; init; }

    [MaxLength( 200 )]
    public string Description { get; init; }

    [Required]
    [MaxLength( 50 )]
    public string Login { get; init; }

    public string OldPassword { get; init; }

    public string NewPassword { get; init; }
}
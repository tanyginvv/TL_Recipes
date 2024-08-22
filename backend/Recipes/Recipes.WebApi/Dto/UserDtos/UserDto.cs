using System.ComponentModel.DataAnnotations;

namespace Recipes.WebApi.Dto.UserDtos;

public class UserDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    [MaxLength( 50 )]
    public string Name { get; init; }

    [Required]
    [MaxLength( 200 )]
    public string Description { get; init; }

    [Required]
    [MaxLength( 50 )]
    public string Login { get; init; }

    [Required]
    public int RecipesCount { get; init; }
}
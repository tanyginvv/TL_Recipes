using System.ComponentModel.DataAnnotations;

namespace Recipes.WebApi.Dto.UserDtos;

public class ReadUserDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    public string Name { get; init; }
}
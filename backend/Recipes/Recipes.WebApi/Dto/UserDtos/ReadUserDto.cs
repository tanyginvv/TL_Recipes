using System.ComponentModel.DataAnnotations;

namespace Recipes.WebApi.Dto.UserDtos;

public class ReadUserDto
{
    [Required]
    public string Name { get; init; }
}
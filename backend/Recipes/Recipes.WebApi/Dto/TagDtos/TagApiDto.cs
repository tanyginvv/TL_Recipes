using System.ComponentModel.DataAnnotations;

namespace Recipes.WebApi.Dto.TagDtos
{
    public class TagApiDto
    {
        [Required]
        public string Name { get; init; }
    }
}
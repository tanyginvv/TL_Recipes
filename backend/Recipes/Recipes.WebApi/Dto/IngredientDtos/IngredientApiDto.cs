using System.ComponentModel.DataAnnotations;

namespace Recipes.WebApi.Dto.IngredientDtos
{
    public class IngredientApiDto
    {
        [Required]
        public string Title { get; init; }

        [Required]
        public string Description { get; init; }
    }
}
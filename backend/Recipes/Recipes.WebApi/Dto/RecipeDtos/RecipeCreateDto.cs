using System.ComponentModel.DataAnnotations;
using Recipes.Application.UseCases.Recipes.Dtos;

namespace Recipes.WebApi.Dto.RecipeDtos
{
    public class RecipeCreateDto
    {
        [Required]
        [StringLength( 50 )]
        public string Name { get; set; }

        [Required]
        [StringLength( 250 )]
        public string Description { get; set; }

        [Required]
        public int CookTime { get; set; }

        [Required]
        public int PortionCount { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public List<IngredientDto> Ingredients { get; set; }

        [Required]
        public List<StepDto> Steps { get; set; }

        [Required]
        public List<TagDto> Tags { get; set; }
    }
}
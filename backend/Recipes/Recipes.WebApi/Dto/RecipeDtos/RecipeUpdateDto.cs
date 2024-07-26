using System.ComponentModel.DataAnnotations;
using Recipes.Application.UseCases.Recipes.Dtos;

namespace Recipes.WebApi.Dto.RecipeDtos
{
    public class RecipeUpdateDto
    {
        [Required]
        [StringLength( 50 )]
        public string Name { get; init; }

        [Required]
        [StringLength( 250 )]
        public string Description { get; init; }

        [Required]
        public int CookTime { get; init; }

        [Required]
        public int PortionCount { get; init; }

        [Required]
        public string ImageUrl { get; init; }

        [Required]
        public ICollection<IngredientDto> Ingredients { get; init; }

        [Required]
        public ICollection<StepDto> Steps { get; init; }

        [Required]
        public ICollection<TagDto> Tags { get; init; }
    }
}
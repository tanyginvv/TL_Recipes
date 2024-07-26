using System.ComponentModel.DataAnnotations;
using Recipes.Application.UseCases.Recipes.Dtos;

namespace Recipes.WebApi.Dto.RecipeDtos
{
    public class RecipeReadDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength( 50 )]
        public string Name { get; set; }

        [Required]
        [StringLength( 250 )]
        public string Description { get; set; }

        [Required]
        public int CookTime { get; init; }

        [Required]
        public int CountPortion { get; init; }

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
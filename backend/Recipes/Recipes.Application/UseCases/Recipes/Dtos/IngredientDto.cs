using System.ComponentModel.DataAnnotations;

namespace Recipes.Application.UseCases.Recipes.Dtos
{
    public class IngredientDto
    {
        [Required]
        [StringLength( 50 )]
        public string Title { get; set; }

        [Required]
        [StringLength( 250 )]
        public string Description { get; set; }
    }
}

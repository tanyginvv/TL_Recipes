using System.ComponentModel.DataAnnotations;

namespace Recipes.Application.UseCases.Recipes.Dtos
{
    public class StepDto
    {
        [Required]
        public int StepNumber { get; set; }

        [Required]
        [StringLength( 250 )]
        public string StepDescription { get; set; }
    }
}

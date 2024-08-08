using System.ComponentModel.DataAnnotations;

namespace Recipes.WebApi.Dto.StepDtos
{
    public class StepApiDto
    {
        [Required]
        public int StepNumber { get; init; }

        [Required]
        public string StepDescription { get; init; }
    }
}
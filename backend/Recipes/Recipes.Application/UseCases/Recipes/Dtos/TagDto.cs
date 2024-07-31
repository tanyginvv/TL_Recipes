using System.ComponentModel.DataAnnotations;

namespace Recipes.Application.UseCases.Recipes.Dtos
{
    public class TagDto
    {
        [Required]
        [StringLength( 50 )]
        public string Name { get; set; }
    }
}

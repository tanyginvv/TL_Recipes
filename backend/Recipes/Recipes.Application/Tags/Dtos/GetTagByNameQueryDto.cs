using Recipes.Domain.Entities;

namespace Recipes.Application.Tags.Dtos
{
    public class GetTagByNameQueryDto
    {
        public required Tag Tag { get; set; }
    }
}

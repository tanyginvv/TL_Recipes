using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags.Dtos;

public class GetTagByNameQueryDto
{
    public required Tag Tag { get; set; }
}
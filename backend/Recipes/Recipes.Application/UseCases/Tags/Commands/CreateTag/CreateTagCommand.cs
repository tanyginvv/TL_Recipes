using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags.Commands.CreateTag
{
    public class CreateTagCommand
    {
        public required string Name { get; init; }
    }
}

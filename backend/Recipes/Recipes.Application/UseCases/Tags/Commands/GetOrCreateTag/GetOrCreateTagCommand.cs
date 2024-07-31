using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags.Commands
{
    public class GetOrCreateTagCommand
    {
        public required string Name { get; init; }
    }
}

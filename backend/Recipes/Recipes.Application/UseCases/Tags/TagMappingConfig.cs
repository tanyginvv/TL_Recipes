using Mapster;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Tags.Commands;

namespace Recipes.Application.UseCases.Tags
{
    public class StepMappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<TagDto, GetOrCreateTagCommand>.NewConfig();
        }
    }
}

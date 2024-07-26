using Mapster;
using Recipes.Application.UseCases.Steps.Commands;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps
{
    public class StepMappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<CreateStepCommand, Step>.NewConfig()
                .Map( dest => dest.RecipeId, src => src.Recipe.Id );
        }
    }
}

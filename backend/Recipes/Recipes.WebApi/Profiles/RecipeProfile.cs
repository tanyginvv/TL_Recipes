using Mapster;
using Recipes.Application.UseCases.Recipes.Commands.CreateRecipe;
using Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe;
using Recipes.WebApi.Dto.RecipeDtos;

namespace Recipes.WebApi.Profiles;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<RecipeCreateDto, CreateRecipeCommand>.NewConfig();

        TypeAdapterConfig<RecipeUpdateDto, UpdateRecipeCommand>.NewConfig()
            .Ignore( dest => dest.Id );
    }
}
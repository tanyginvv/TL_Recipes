using Mapster;
using Recipes.Application.UseCases.Recipes.Commands.CreateRecipe;
using Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.WebApi.Dto.RecipeDtos;

namespace Recipes.WebApi.Profiles
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<RecipeCreateDto, CreateRecipeCommand>.NewConfig();

            TypeAdapterConfig<RecipeUpdateDto, UpdateRecipeCommand>.NewConfig()
                .Ignore( dest => dest.Id );

            TypeAdapterConfig<RecipeDto, RecipeUpdateDto>.NewConfig();
            TypeAdapterConfig<RecipeDto, RecipeCreateDto>.NewConfig();
        }
    }
}
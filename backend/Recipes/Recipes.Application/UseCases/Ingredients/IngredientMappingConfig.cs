using Mapster;
using Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;
using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients;

public class IngredientMappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<IngredientDto, CreateIngredientCommand>.NewConfig()
            .Map( dest => dest.Title, src => src.Title )
            .Map( dest => dest.Description, src => src.Description );

        TypeAdapterConfig<IngredientDto, UpdateIngredientCommand>.NewConfig()
            .Map( dest => dest.Title, src => src.Title )
            .Map( dest => dest.Description, src => src.Description );

        TypeAdapterConfig<UpdateIngredientCommand, Ingredient>.NewConfig()
            .Map( dest => dest.Id, src => src.Id )
            .Map( dest => dest.Title, src => src.Title )
            .Map( dest => dest.Description, src => src.Description );
    }
}
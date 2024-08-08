using Mapster;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Tags.Commands.GetOrCreateTag;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Recipes;

public static class RecipeMappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<TagDto, GetOrCreateTagCommand>.NewConfig();

        TypeAdapterConfig<Recipe, GetRecipePartDto>.NewConfig()
        .Map( dest => dest.Tags, src => src.Tags.Select( tag => tag.Adapt<TagDto>() ) );

        TypeAdapterConfig<Tag, TagDto>.NewConfig();

        TypeAdapterConfig<Recipe, GetRecipeByIdQueryDto>.NewConfig()
        .Map( dest => dest.Steps, src => src.Steps.Adapt<List<StepDto>>() )
        .Map( dest => dest.Ingredients, src => src.Ingredients.Adapt<List<IngredientDto>>() )
        .Map( dest => dest.Tags, src => src.Tags.Adapt<List<TagDto>>() );
    }
}
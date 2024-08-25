using Mapster;
using Recipes.Application.UseCases.Recipes.Commands.CreateRecipe;
using Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipes;
using Recipes.WebApi.Dto.RecipeDtos;

namespace Recipes.WebApi.Profiles;

public static class RecipeMappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<RecipeCreateDto, CreateRecipeCommand>
        .NewConfig()
        .Ignore( dest => dest.AuthorId )
        .Map( dest => dest.Tags, src => src.Tags.Adapt<ICollection<TagDto>>() )
        .Map( dest => dest.Steps, src => src.Steps.Adapt<ICollection<StepDto>>() )
        .Map( dest => dest.Ingredients, src => src.Ingredients.Adapt<ICollection<IngredientDto>>() );


        TypeAdapterConfig<RecipeUpdateDto, UpdateRecipeCommand>
            .NewConfig()
            .Ignore( dest => dest.AuthorId )
            .Map( dest => dest.Ingredients, src => src.Ingredients.Adapt<ICollection<IngredientDto>>() )
            .Map( dest => dest.Steps, src => src.Steps.Adapt<ICollection<StepDto>>() )
            .Map( dest => dest.Tags, src => src.Tags.Adapt<ICollection<TagDto>>() );

        TypeAdapterConfig<RecipeDto, RecipeUpdateDto>.NewConfig();
        TypeAdapterConfig<RecipeDto, RecipeCreateDto>.NewConfig();

        TypeAdapterConfig<GetRecipesDto, GetRecipesQuery>
            .NewConfig()
            .Ignore( dest => dest.UserId )
            .Map( dest => dest.SearchTerms, src => src.SearchTerms )
            .Map( dest => dest.PageNumber, src => src.PageNumber );
    }
}
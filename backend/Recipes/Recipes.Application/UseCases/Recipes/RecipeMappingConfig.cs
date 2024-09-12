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

        TypeAdapterConfig<Tag, TagDto>.NewConfig();

        TypeAdapterConfig<Recipe, GetRecipeQueryDto>.NewConfig()
           .Map( dest => dest.Steps, src => src.Steps.Adapt<List<StepDto>>() )
           .Map( dest => dest.Ingredients, src => src.Ingredients.Adapt<List<IngredientDto>>() )
           .Map( dest => dest.Tags, src => src.Tags.Adapt<List<TagDto>>() )
           .Map( dest => dest.AuthorLogin, src => src.Author != null ? src.Author.Login : string.Empty )
           .Map( dest => dest.FavouriteCount, src => src.Favourites.Count )
           .Map( dest => dest.LikeCount, src => src.Likes.Count );

        TypeAdapterConfig<Recipe, GetRecipePartDto>.NewConfig()
           .Map( dest => dest.AuthorLogin, src => src.Author != null ? src.Author.Login : string.Empty )
           .Map( dest => dest.LikeCount, src => src.Likes.Count )
           .Map( dest => dest.FavouriteCount, src => src.Favourites.Count )
           .Map( dest => dest.Tags, src => src.Tags.Select( tag => tag.Adapt<TagDto>() ) );

        TypeAdapterConfig<Recipe, GetRecipePartDto>.NewConfig()
           .Map( dest => dest.AuthorLogin, src => src.Author != null ? src.Author.Login : string.Empty )
           .Map( dest => dest.LikeCount, src => src.Likes.Count )
           .Map( dest => dest.FavouriteCount, src => src.Favourites.Count )
           .Map( dest => dest.Tags, src => src.Tags.Select( tag => tag.Adapt<TagDto>() ).ToList() );

        TypeAdapterConfig<Recipe, GetRecipeOfDayDto>.NewConfig()
           .Map( dest => dest.AuthorLogin, src => src.Author != null ? src.Author.Login : string.Empty )
           .Map( dest => dest.LikeCount, src => src.Likes.Count );
    }
}
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Likes.Dtos;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Likes.Queries.GetLikesCountForRecipeQuery
{
    public class GetFavouritesCountForRecipeQueryHandler(
        IFavouriteRepository favouriteRepository,
        IAsyncValidator<GetFavouritesCountForRecipeQuery> validator )
        : IQueryHandler<FavouritesCountDto, GetFavouritesCountForRecipeQuery>
    {
        public async Task<Result<FavouritesCountDto>> HandleAsync( GetFavouritesCountForRecipeQuery command )
        {
            Result result = await validator.ValidateAsync( command );
            if ( !result.IsSuccess )
            {
                return Result<FavouritesCountDto>.FromError( result );
            }

            int count = await favouriteRepository.GetFavouritesCount( command.RecipeId );

            FavouritesCountDto favouritesCountDto = new() { Count = count };

            return Result<FavouritesCountDto>.FromSuccess( favouritesCountDto );
        }
    }
}

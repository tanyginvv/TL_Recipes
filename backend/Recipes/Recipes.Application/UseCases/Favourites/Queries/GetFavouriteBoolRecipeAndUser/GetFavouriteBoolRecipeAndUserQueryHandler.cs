using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Favourites.Dtos;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Likes.Queries.GetLikeBoolRecipeAndUser
{
    public class GetFavouriteBoolRecipeAndUserQueryHandler(
        IFavouriteRepository favouriteRepository,
        IAsyncValidator<GetFavouriteBoolRecipeAndUserQuery> validator )
        : IQueryHandler<FavouriteBoolDto, GetFavouriteBoolRecipeAndUserQuery>
    {
        public async Task<Result<FavouriteBoolDto>> HandleAsync( GetFavouriteBoolRecipeAndUserQuery command )
        {
            Result result = await validator.ValidateAsync( command );
            if ( !result.IsSuccess )
            {
                return Result<FavouriteBoolDto>.FromError( result );
            }

            bool isFavourite = await favouriteRepository.ContainsAsync( u => u.UserId == command.UserId && u.RecipeId == command.RecipeId );

            FavouriteBoolDto favouriteBoolDto = new() { IsFavourite = isFavourite };

            return Result<FavouriteBoolDto>.FromSuccess( favouriteBoolDto );
        }
    }
}

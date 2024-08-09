using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Likes.Dtos;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Likes.Queries.GetLikeBoolRecipeAndUser;

public class GetLikeBoolRecipeAndUserQueryHandler(
    ILikeRepository likeRepository,
    IAsyncValidator<GetLikeBoolRecipeAndUserQuery> validator )
    : IQueryHandler<LikeBoolDto, GetLikeBoolRecipeAndUserQuery>
{
    public async Task<Result<LikeBoolDto>> HandleAsync( GetLikeBoolRecipeAndUserQuery command )
    {
        Result result = await validator.ValidateAsync( command );
        if ( !result.IsSuccess )
        {
            return Result<LikeBoolDto>.FromError( result );
        }

        bool isLiked = await likeRepository.ContainsAsync( u => u.UserId == command.UserId && u.RecipeId == command.RecipeId );

        LikeBoolDto likeBoolDto = new() { IsLiked = isLiked };

        return Result<LikeBoolDto>.FromSuccess( likeBoolDto );
    }
}

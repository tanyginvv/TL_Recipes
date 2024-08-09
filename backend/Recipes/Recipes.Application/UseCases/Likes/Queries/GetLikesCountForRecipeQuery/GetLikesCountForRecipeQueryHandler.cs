using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Likes.Dtos;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Likes.Queries.GetLikesCountForRecipeQuery;

public class GetLikesCountForRecipeQueryHandler(
    ILikeRepository likeRepository,
    IAsyncValidator<GetLikesCountForRecipeQuery> validator )
    : IQueryHandler<LikesCountDto, GetLikesCountForRecipeQuery>
{
    public async Task<Result<LikesCountDto>> HandleAsync( GetLikesCountForRecipeQuery command )
    {
        Result result = await validator.ValidateAsync( command );
        if ( !result.IsSuccess )
        {
            return Result<LikesCountDto>.FromError( result );
        }

        int count = await likeRepository.GetLikesCount( command.RecipeId );

        LikesCountDto likesCountDto = new() { Count = count };

        return Result<LikesCountDto>.FromSuccess( likesCountDto );
    }
}

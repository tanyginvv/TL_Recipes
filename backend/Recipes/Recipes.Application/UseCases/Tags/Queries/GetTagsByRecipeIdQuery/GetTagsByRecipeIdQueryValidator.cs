using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Tags.Queries.GetTagsByRecipeIdQuery
{
    public class GetTagsByRecipeIdQueryValidator 
        : IAsyncValidator<GetTagsByRecipeIdQuery>
    {
        public async Task<Result> ValidateAsync( GetTagsByRecipeIdQuery query )
        {
            if ( query.RecipeId <= 0 )
            {
                return Result.FromError( "Id рецепта должен быть больше 0" );
            }

            return Result.Success;
        }
    }
}
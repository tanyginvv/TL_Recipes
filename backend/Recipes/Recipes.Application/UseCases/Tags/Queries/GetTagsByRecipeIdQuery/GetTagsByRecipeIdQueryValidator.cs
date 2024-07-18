using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Tags.Queries.GetTagsByRecipeIdQuery
{
    public class GetTagsByRecipeIdQueryValidator : IAsyncValidator<GetTagsByRecipeIdQuery>
    {
        public async Task<Result> ValidationAsync( GetTagsByRecipeIdQuery query )
        {
            if ( query.RecipeId <= 0 )
            {
                return Result.FromError( "Recipe ID must be greater than zero." );
            }

            return Result.Success;
        }
    }
}

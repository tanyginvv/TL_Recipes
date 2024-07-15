using Recipes.Application.Validation;

namespace Recipes.Application.Tags.Queries.GetTagsByRecipeIdQuery
{
    public class GetTagsByRecipeIdQueryValidator : IAsyncValidator<GetTagsByRecipeIdQuery>
    {
        public async Task<ValidationResult> ValidationAsync( GetTagsByRecipeIdQuery query )
        {
            if ( query.RecipeId <= 0 )
            {
                return ValidationResult.Fail( "Recipe ID must be greater than zero." );
            }

            return ValidationResult.Ok();
        }
    }
}

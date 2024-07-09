using Application.Validation;

namespace Recipes.Application.Recipes.Queries.GetAllRecipes
{
    public class GetAllRecipesQueryValidator : IAsyncValidator<GetAllRecipesQuery>
    {
        public Task<ValidationResult> ValidationAsync( GetAllRecipesQuery query )
        {

            return Task.FromResult( ValidationResult.Ok() );
        }
    }
}

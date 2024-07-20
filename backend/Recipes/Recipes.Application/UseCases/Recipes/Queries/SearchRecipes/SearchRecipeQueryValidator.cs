using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Recipes.Queries.SearchRecipes
{
    public class SearchRecipesQueryValidator : IAsyncValidator<SearchRecipesQuery>
    {
        public async Task<Result> ValidationAsync( SearchRecipesQuery query )
        {
            if ( query.SearchTerms == null || !query.SearchTerms.Any() )
            {
                return Result.FromError( "Список поисковых терминов не должен быть пустым." );
            }

            foreach ( var term in query.SearchTerms )
            {
                if ( string.IsNullOrWhiteSpace( term ) )
                {
                    return Result.FromError( "Поисковые термины не должны быть пустыми строками или состоять только из пробелов." );
                }
            }

            return Result.Success;
        }
    }

}

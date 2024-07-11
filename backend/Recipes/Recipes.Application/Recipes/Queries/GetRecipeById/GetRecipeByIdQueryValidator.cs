using Application.Repositories;
using Application.Validation;

namespace Recipes.Application.Recipes.Queries.GetRecipe
{
    public class GetRecipeByIdQueryValidator : IAsyncValidator<GetRecipeByIdQuery>
    {
        private readonly IRecipeRepository _repository;

        public GetRecipeByIdQueryValidator( IRecipeRepository repository )
        {
            _repository = repository;
        }

        public async Task<ValidationResult> ValidationAsync( GetRecipeByIdQuery query )
        {
            if ( query.Id <= 0 )
            {
                return ValidationResult.Fail( "Id рецепта меньше нуля" );
            }

            if ( await _repository.GetByIdAsync( query.Id ) != null )
            {
                return ValidationResult.Fail( "Рецепта с этим Id не существует" );
            }

            return ValidationResult.Ok();
        }
    }
}
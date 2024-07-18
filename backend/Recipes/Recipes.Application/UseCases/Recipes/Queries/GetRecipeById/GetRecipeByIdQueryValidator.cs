using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipeById;
using Recipes.Application.Validation;

public class GetRecipeByIdQueryValidator( IRecipeRepository repository ) : IAsyncValidator<GetRecipeByIdQuery>
{
    public async Task<Result> ValidationAsync( GetRecipeByIdQuery query )
    {
        if ( query.Id <= 0 )
        {
            return Result.FromError( "Id рецепта меньше нуля" );
        }

        if ( await repository.GetByIdAsync( query.Id ) == null )
        {
            return Result.FromError( "Рецепта с этим Id не существует" );
        }

        return Result.Success;
    }
}
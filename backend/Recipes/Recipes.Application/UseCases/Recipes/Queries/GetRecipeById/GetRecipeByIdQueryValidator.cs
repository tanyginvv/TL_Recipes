using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipeById;

public class GetRecipeByIdQueryValidator( 
    IRecipeRepository repository ) 
    : IAsyncValidator<GetRecipeByIdQuery>
{
    public async Task<Result> ValidateAsync( GetRecipeByIdQuery query )
    {
        if ( query.Id <= 0 )
        {
            return Result.FromError( "Id рецепта меньше нуля" );
        }

        if ( await repository.GetByIdAsync( query.Id ) is null ) 
        {
            return Result.FromError( "Рецепт не найден" );
        }

        return Result.Success;
    }
}
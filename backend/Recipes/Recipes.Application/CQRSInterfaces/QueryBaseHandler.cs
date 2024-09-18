using Microsoft.Extensions.Logging;
using Recipes.Application.Results;

namespace Recipes.Application.CQRSInterfaces;

public abstract class QueryBaseHandler<TResult, TQuery>( IAsyncValidator<TQuery> validator, ILogger<TQuery> logger )
: IQueryHandler<TResult, TQuery>
where TResult : class
where TQuery : class
{
    public async Task<Result<TResult>> HandleAsync( TQuery query )
    {
        Result validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsSuccess )
        {
            return Result<TResult>.FromError( validationResult.Error );
        }

        try
        {
            return await HandleImplAsync( query );
        }
        catch ( Exception ex )
        {
            logger.LogError( ex, "Error handling command of type {QueryType}.", typeof( TQuery ).Name );
            return Result<TResult>.FromError( ex.Message );
        }
    }

    protected abstract Task<Result<TResult>> HandleImplAsync( TQuery query );
}
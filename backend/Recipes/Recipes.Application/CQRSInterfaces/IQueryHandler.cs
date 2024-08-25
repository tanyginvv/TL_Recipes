using Recipes.Application.Results;

namespace Recipes.Application.CQRSInterfaces;

public interface IQueryHandler<TResult, TQuery>
    where TResult : class
    where TQuery : class
{
    Task<Result<TResult>> HandleAsync( TQuery query );
}

public abstract class QueryBaseHandler<TResult, TQuery>( IAsyncValidator<TQuery> validator )
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
            return await HandleAsyncImpl( query );
        }
        catch ( Exception ex )
        {
            return Result<TResult>.FromError( ex.Message );
        }
    }

    protected abstract Task<Result<TResult>> HandleAsyncImpl( TQuery query );
}
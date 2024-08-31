using Recipes.Application.Results;

namespace Recipes.Application.CQRSInterfaces;

public abstract class CommandBaseHandlerWithResult<TCommand, TResult>( IAsyncValidator<TCommand> validator )
    : ICommandHandlerWithResult<TCommand, TResult> where TCommand : class
{
    public virtual async Task<Result<TResult>> HandleAsync( TCommand command )
    {
        Result validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsSuccess )
        {
            await HandleExceptionAsync( command );
            return Result<TResult>.FromError( validationResult.Error );
        }

        try
        {
            return await HandleAsyncImpl( command );
        }
        catch ( Exception ex )
        {
            await HandleExceptionAsync( command );
            return Result<TResult>.FromError( ex.Message );
        }
    }

    protected abstract Task<Result<TResult>> HandleAsyncImpl( TCommand command );
    protected virtual Task HandleExceptionAsync( TCommand command )
    {
        return Task.CompletedTask;
    }
}
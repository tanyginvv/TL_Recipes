using Microsoft.Extensions.Logging;
using Recipes.Application.Results;

namespace Recipes.Application.CQRSInterfaces;

public abstract class CommandBaseHandlerWithResult<TCommand, TResult>( IAsyncValidator<TCommand> validator, ILogger<TCommand> logger )
    : ICommandHandlerWithResult<TCommand, TResult> where TCommand : class
{
    public virtual async Task<Result<TResult>> HandleAsync( TCommand command )
    {
        Result validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsSuccess )
        {
            await CleanupOnFailureAsync( command );
            return Result<TResult>.FromError( validationResult.Error );
        }

        try
        {
            return await HandleImplAsync( command );
        }
        catch ( Exception ex )
        {
            logger.LogError( ex, "Error handling command of type {CommandType}.", typeof( TCommand ).Name );
            await CleanupOnFailureAsync( command );
            return Result<TResult>.FromError( ex.Message );
        }
    }

    protected abstract Task<Result<TResult>> HandleImplAsync( TCommand command );
    protected virtual Task CleanupOnFailureAsync( TCommand command )
    {
        return Task.CompletedTask;
    }
}
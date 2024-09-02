using Recipes.Application.Results;

namespace Recipes.Application.CQRSInterfaces;

public abstract class CommandBaseHandler<TCommand>( IAsyncValidator<TCommand> validator )
    : ICommandHandler<TCommand> where TCommand : class
{
    public virtual async Task<Result> HandleAsync( TCommand command )
    {
        Result validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsSuccess )
        {
            await CleanupOnFailureAsync( command );
            return Result.FromError( validationResult.Error );
        }

        try
        {
            await HandleImplAsync( command );
            return Result.Success;
        }
        catch ( Exception ex )
        {
            await CleanupOnFailureAsync( command );
            return Result.FromError( ex.Message );
        }
    }

    protected abstract Task HandleImplAsync( TCommand command );
    protected virtual Task CleanupOnFailureAsync( TCommand command )
    {
        return Task.CompletedTask;
    }
}
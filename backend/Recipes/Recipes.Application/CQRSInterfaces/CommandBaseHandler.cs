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
            await HandleExceptionAsync( command );
            return Result.FromError( validationResult.Error );
        }

        try
        {
            await HandleAsyncImpl( command );
            return Result.Success;
        }
        catch ( Exception ex )
        {
            await HandleExceptionAsync( command );
            return Result.FromError( ex.Message );
        }
    }

    protected abstract Task HandleAsyncImpl( TCommand command );
    protected virtual Task HandleExceptionAsync( TCommand command )
    {
        return Task.CompletedTask;
    }
}
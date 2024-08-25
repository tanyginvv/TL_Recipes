using Recipes.Application.Results;

namespace Recipes.Application.CQRSInterfaces;

public interface ICommandHandler<TCommand> where TCommand : class
{
    Task<Result> HandleAsync( TCommand command );
}
public interface ICommandHandlerWithResult<TCommand, TResult> where TCommand : class
{
    Task<Result<TResult>> HandleAsync( TCommand command );
}

public abstract class CommandBaseHandler<TCommand, TResult>( IAsyncValidator<TCommand> validator ) 
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
            return Result<TResult>.FromError( ex.Message );
        }
    }

    protected abstract Task<Result<TResult>> HandleAsyncImpl( TCommand command );
    protected virtual Task HandleExceptionAsync( TCommand command )
    {
        return Task.CompletedTask;
    }
}

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
            return Result.FromError( ex.Message );
        }
    }

    protected abstract Task HandleAsyncImpl( TCommand command );
    protected virtual Task HandleExceptionAsync( TCommand command )
    {
        return Task.CompletedTask;
    }
}
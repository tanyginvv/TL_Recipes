using Recipes.Application.Results;

namespace Recipes.Application.CQRSInterfaces;

public interface ICommandHandlerWithResult<TCommand, TResult> where TCommand : class
{
    Task<Result<TResult>> HandleAsync( TCommand command );
}
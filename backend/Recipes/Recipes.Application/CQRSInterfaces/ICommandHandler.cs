using Recipes.Application.Results;

namespace Recipes.Application.CQRSInterfaces
{

    public interface ICommandHandler<TCommand> where TCommand : class
    {
        Task<Result> HandleAsync( TCommand command );
    }
}

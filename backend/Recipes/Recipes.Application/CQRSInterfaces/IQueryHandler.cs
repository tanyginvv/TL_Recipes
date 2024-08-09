using Recipes.Application.Results;

namespace Recipes.Application.CQRSInterfaces;

public interface IQueryHandler<TResult, TQuery>
    where TResult : class
    where TQuery : class
{
    Task<Result<TResult>> HandleAsync( TQuery query );
}
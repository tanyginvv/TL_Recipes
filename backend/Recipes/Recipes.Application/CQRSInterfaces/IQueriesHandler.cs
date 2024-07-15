using Recipes.Application.Recipes.Dtos;
using Recipes.Application.Results;

namespace Recipes.Application.CQRSInterfaces
{
    public interface IQueryHandler<TResult, TQuery>
        where TResult : class
        where TQuery : class
    {
        Task<QueryResult<TResult>> HandleAsync( TQuery query );
    }
}
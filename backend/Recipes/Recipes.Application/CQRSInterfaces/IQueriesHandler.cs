using Application.Result;
using Recipes.Application.Recipes.Dtos;

namespace Application.CQRSInterfaces
{
    public interface IQueryHandler<TResult, TQuery>
        where TResult : class
        where TQuery : class
    {
        Task<QueryResult<TResult>> HandleAsync( TQuery query );
    }
}
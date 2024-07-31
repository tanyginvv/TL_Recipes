using System.Linq.Expressions;

namespace Recipes.Application.Repositories
{
    public interface ISearchRepository<TEntity> where TEntity : class
    {
        Task<bool> ContainsAsync( Expression<Func<TEntity, bool>> predicate );
    }
}
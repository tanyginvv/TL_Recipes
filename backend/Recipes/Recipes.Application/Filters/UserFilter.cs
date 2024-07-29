using Recipes.Application.Interfaces;
using Recipes.Domain.Entities;

namespace Recipes.Application.Filters
{
    public class UserFilter : IFilter<User>
    {
        public int UserId { get; set; }

        public IQueryable<User> Apply( IQueryable<User> query )
        {
            query = query.Where( u => u.Id == UserId );

            return query;
        }
    }
}

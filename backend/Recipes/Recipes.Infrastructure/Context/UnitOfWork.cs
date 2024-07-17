using Recipes.Application.Repositories;

namespace Recipes.Infrastructure.Context
{
    public class UnitOfWork( RecipesDbContext dbContext ) : IUnitOfWork
    {
        private RecipesDbContext _dbContext => dbContext;

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

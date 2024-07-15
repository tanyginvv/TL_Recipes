using Recipes.Application.Repositories;

namespace Recipes.Infrastructure.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RecipesDbContext _dbContext;

        public UnitOfWork( RecipesDbContext dbContext )
        {
            _dbContext = dbContext;
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

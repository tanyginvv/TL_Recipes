using Recipes.Application.Repositories;

namespace Recipes.Infrastructure.Context
{
    public class UnitOfWork( RecipesDbContext dbContext ) : IUnitOfWork
    {
        public async Task CommitAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}

using Recipes.Application.Interfaces;

namespace Recipes.Infrastructure.DataAccess;

public class UnitOfWork( RecipesDbContext dbContext ) : IUnitOfWork
{
    public async Task CommitAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}
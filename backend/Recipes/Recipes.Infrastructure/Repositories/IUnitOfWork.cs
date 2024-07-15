namespace Recipes.Infrastructure.Repositories
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
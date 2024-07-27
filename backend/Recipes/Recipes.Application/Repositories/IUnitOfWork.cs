namespace Recipes.Application.Repositories
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
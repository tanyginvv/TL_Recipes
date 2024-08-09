namespace Recipes.Application.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync();
}
namespace Application
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
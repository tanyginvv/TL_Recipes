namespace Application.Repositories.BaseRepositories
{
    public interface IAddedRepository<TEntity> where TEntity : class
    {
        void Add( TEntity entity );
    }
}
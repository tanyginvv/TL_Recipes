namespace Application.Repositories.BaseRepositories
{
    public interface IRemovableRepository<TEntity> where TEntity : class
    {
        void Delete( TEntity entety );
    }
}

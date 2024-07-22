namespace Recipes.Application.Repositories
{
    public interface ISpecification<T>
    {
        IQueryable<T> Apply( IQueryable<T> query );
    }
}

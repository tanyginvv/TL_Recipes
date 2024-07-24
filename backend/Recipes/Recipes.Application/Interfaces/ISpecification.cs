namespace Recipes.Application.Interfaces
{
    public interface ISpecification<T>
    {
        IQueryable<T> Apply( IQueryable<T> query );
    }
}

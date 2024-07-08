using Application.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using Recipes.Infrastructure.Context;

namespace Recipes.Infrastructure.Entities
{
    public abstract class BaseRepository<TEntity> : IAddedRepository<TEntity>,
        IRemovableRepository<TEntity> where TEntity : class
    {
        protected readonly RecipesDbContext DBContext;
        private DbContext _context;

        protected DbSet<TEntity> Entities => DBContext.Set<TEntity>();

        public BaseRepository( RecipesDbContext dbContext )
        {
            DBContext = dbContext;
        }

        protected BaseRepository( DbContext context )
        {
            _context = context;
        }

        public void Add( TEntity entity )
        {
            Entities.Add( entity );
        }
        public void Delete( TEntity entity )
        {
            Entities.Remove( entity );
        }
    }
}
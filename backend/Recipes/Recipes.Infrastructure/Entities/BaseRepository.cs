using Microsoft.EntityFrameworkCore;

public abstract class BaseRepository<TEntity> where TEntity : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected BaseRepository( DbContext context )
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<TEntity> GetByIdAsync( int id )
    {
        return await _dbSet.FindAsync( id );
    }

    public virtual async Task AddAsync( TEntity entity )
    {
        await _dbSet.AddAsync( entity );
        await _context.SaveChangesAsync();
    }

    public virtual async Task Update( TEntity entity )
    {
        _dbSet.Update( entity );
        await _context.SaveChangesAsync();
    }

    public virtual void Remove( TEntity entity )
    {
        _dbSet.Remove( entity );
    }
}
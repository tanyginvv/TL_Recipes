using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Repositories;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Context;

namespace Recipes.Infrastructure.Entities.Users;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository( RecipesDbContext context ) : base( context )
    {
    }

    public async Task<bool> ContainsAsync( Expression<Func<User, bool>> predicate )
    {
        return await _context.Set<User>().AnyAsync( predicate );
    }

    public async Task Delete( User entity )
    {
        User user = await GetByIdAsync( entity.Id );
        if ( user is not null )
        {
            base.Remove( entity );
        }
    }

    public async Task<User> GetByIdAsync( int id )
    {
        return await _context.Set<User>()
            .Include( u => u.Recipes )
            .Include( u => u.Favourites )
            .Include( u => u.Likes )
            .FirstOrDefaultAsync( u => u.Id == id );
    }

    public async Task<User> GetByLoginAsync( string login )
    {
        return await _context.Set<User>().FirstOrDefaultAsync( user => user.Login == login );
    }

    public async Task<IReadOnlyList<Recipe>> GetRecipesAsync( int id )
    {
        User user = await _context.Set<User>()
                                 .Include( u => u.Recipes )
                                 .FirstOrDefaultAsync( u => u.Id == id );
        return user.Recipes.ToList().AsReadOnly();
    }
}

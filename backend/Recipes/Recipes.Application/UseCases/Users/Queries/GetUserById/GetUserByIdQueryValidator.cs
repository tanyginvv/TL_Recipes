using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Users.Queries.GetUserById;

public class GetUserByIdQueryValidator( IUserRepository userRepository ) : IAsyncValidator<GetUserByIdQuery>
{
    public async Task<Result> ValidateAsync( GetUserByIdQuery query )
    {
        if ( !await userRepository.ContainsAsync( user => user.Id == query.Id ) )
        {
            return Result.FromError( "Пользователя с таким id нет" );
        }

        return Result.FromSuccess();
    }
}
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Queries.GetUserById;
using Recipes.Application.Validation;

namespace Application.Users.Queries.GetUserById;

public class GetUserLoginByIdQueryValidator(
    IUserRepository userRepository )
    : IAsyncValidator<GetUserLoginByIdQuery>
{
    public async Task<Result> ValidateAsync( GetUserLoginByIdQuery query )
    {
        if ( !await userRepository.ContainsAsync( user => user.Id == query.Id ) )
        {
            return Result.FromError( "Пользователя с таким id нет" );
        }

        return Result.FromSuccess();
    }
}
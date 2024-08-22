using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Users.Queries.GetUserNameById;

public class GetUserNameByIdQueryValidator(
    IUserRepository userRepository )
    : IAsyncValidator<GetUserNameByIdQuery>
{
    public async Task<Result> ValidateAsync( GetUserNameByIdQuery query )
    {
        if ( !await userRepository.ContainsAsync( user => user.Id == query.Id ) )
        {
            return Result.FromError( "Пользователя с таким id нет" );
        }

        return Result.FromSuccess();
    }
}
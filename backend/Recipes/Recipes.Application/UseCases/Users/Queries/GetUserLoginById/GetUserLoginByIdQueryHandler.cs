using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.Application.UseCases.Users.Queries.GetUserById;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Application.Users.Queries.GetUserById;

public class GetUserLoginByIdQueryHandler(
    IUserRepository userRepository,
    IAsyncValidator<GetUserLoginByIdQuery> validator )
    : IQueryHandler<GetUserLoginByIdQueryDto, GetUserLoginByIdQuery>
{
    public async Task<Result<GetUserLoginByIdQueryDto>> HandleAsync( GetUserLoginByIdQuery getUserByIdQuery )
    {
        Result validationResult = await validator.ValidateAsync( getUserByIdQuery );
        if ( !validationResult.IsSuccess )
        {
            return Result<GetUserLoginByIdQueryDto>.FromError( validationResult );
        }

        User user = await userRepository.GetByIdAsync( getUserByIdQuery.Id );
        GetUserLoginByIdQueryDto getUserLoginByIdQueryDto = new GetUserLoginByIdQueryDto
        {
            Id = user.Id,
            Login = user.Login
        };
        return Result<GetUserLoginByIdQueryDto>.FromSuccess( getUserLoginByIdQueryDto );
    }
}
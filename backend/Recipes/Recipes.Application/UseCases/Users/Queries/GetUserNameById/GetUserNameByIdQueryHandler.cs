using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Users.Queries.GetUserNameById;

public class GetUserNameByIdQueryHandler(
    IUserRepository userRepository,
    IAsyncValidator<GetUserNameByIdQuery> validator )
    : IQueryHandler<GetUserNameByIdQueryDto, GetUserNameByIdQuery>
{
    public async Task<Result<GetUserNameByIdQueryDto>> HandleAsync( GetUserNameByIdQuery getUserByIdQuery )
    {
        Result validationResult = await validator.ValidateAsync( getUserByIdQuery );
        if ( !validationResult.IsSuccess )
        {
            return Result<GetUserNameByIdQueryDto>.FromError( validationResult );
        }

        User user = await userRepository.GetByIdAsync( getUserByIdQuery.Id );
        GetUserNameByIdQueryDto getUserNameByIdQueryDto = new GetUserNameByIdQueryDto
        {
            Id = user.Id,
            Name = user.Login
        };
        return Result<GetUserNameByIdQueryDto>.FromSuccess( getUserNameByIdQueryDto );
    }
}
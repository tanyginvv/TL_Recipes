using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Users.Queries.GetUserNameById;

public class GetUserNameByIdQueryHandler(
    IUserRepository userRepository,
    IAsyncValidator<GetUserNameByIdQuery> validator )
    : QueryBaseHandler<GetUserNameByIdQueryDto, GetUserNameByIdQuery>( validator )
{
    protected override async Task<Result<GetUserNameByIdQueryDto>> HandleAsyncImpl( GetUserNameByIdQuery query )
    {
        User user = await userRepository.GetByIdAsync( query.Id );
        GetUserNameByIdQueryDto getUserNameByIdQueryDto = new GetUserNameByIdQueryDto
        {
            Name = user.Name
        };
        return Result<GetUserNameByIdQueryDto>.FromSuccess( getUserNameByIdQueryDto );
    }
}
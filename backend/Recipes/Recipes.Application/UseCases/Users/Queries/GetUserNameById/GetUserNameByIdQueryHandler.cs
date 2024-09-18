using Mapster;
using Microsoft.Extensions.Logging;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Users.Queries.GetUserNameById;

public class GetUserNameByIdQueryHandler(
    IUserRepository userRepository,
    IAsyncValidator<GetUserNameByIdQuery> validator,
    ILogger<GetUserNameByIdQuery> logger )
    : QueryBaseHandler<GetUserNameByIdQueryDto, GetUserNameByIdQuery>( validator, logger )
{
    protected override async Task<Result<GetUserNameByIdQueryDto>> HandleImplAsync( GetUserNameByIdQuery query )
    {
        User user = await userRepository.GetByIdAsync( query.Id );
        GetUserNameByIdQueryDto getUserNameByIdQueryDto = user.Adapt<GetUserNameByIdQueryDto>();

        return Result<GetUserNameByIdQueryDto>.FromSuccess( getUserNameByIdQueryDto );
    }
}
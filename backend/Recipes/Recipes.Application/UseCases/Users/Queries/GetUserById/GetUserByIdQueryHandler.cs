using Mapster;
using Microsoft.Extensions.Logging;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(
    IUserRepository userRepository,
    IAsyncValidator<GetUserByIdQuery> validator,
    ILogger<GetUserByIdQuery> logger )
    : QueryBaseHandler<GetUserByIdQueryDto, GetUserByIdQuery>( validator, logger )
{
    protected override async Task<Result<GetUserByIdQueryDto>> HandleImplAsync( GetUserByIdQuery query )
    {
        User user = await userRepository.GetByIdAsync( query.Id );

        GetUserByIdQueryDto getUserByIdQueryDto = user.Adapt<GetUserByIdQueryDto>();

        return Result<GetUserByIdQueryDto>.FromSuccess( getUserByIdQueryDto );
    }
}
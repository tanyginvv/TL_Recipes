using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(
    IUserRepository userRepository,
    IAsyncValidator<GetUserByIdQuery> validator )
    : QueryBaseHandler<GetUserByIdQueryDto, GetUserByIdQuery>( validator )
{
    protected override async Task<Result<GetUserByIdQueryDto>> HandleAsyncImpl( GetUserByIdQuery query )
    {
        User user = await userRepository.GetByIdAsync( query.Id );

        GetUserByIdQueryDto getUserByIdQueryDto = new GetUserByIdQueryDto
        {
            Id = user.Id,
            Name = user.Name,
            Login = user.Login,
            Description = user.Description,
            RecipeCount = user.Recipes.Count(),
        };

        return Result<GetUserByIdQueryDto>.FromSuccess( getUserByIdQueryDto );
    }
}
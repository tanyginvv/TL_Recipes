using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(
    IUserRepository userRepository,
    IAsyncValidator<GetUserByIdQuery> validator )
    : IQueryHandler<GetUserByIdQueryDto, GetUserByIdQuery>
{
    public async Task<Result<GetUserByIdQueryDto>> HandleAsync( GetUserByIdQuery getUserByIdQuery )
    {
        Result validationResult = await validator.ValidateAsync( getUserByIdQuery );
        if ( !validationResult.IsSuccess )
        {
            return Result<GetUserByIdQueryDto>.FromError( validationResult );
        }

        User user = await userRepository.GetByIdAsync( getUserByIdQuery.Id );

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
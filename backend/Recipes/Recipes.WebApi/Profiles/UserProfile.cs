using Mapster;
using Recipes.Application.UseCases.Users.Commands.CreateUser;
using Recipes.Application.UseCases.Users.Commands.UpdateUser;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.WebApi.Dto.UserDtos;

namespace Recipes.WebApi.Profiles;
public static class UserMappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<GetUserByIdQueryDto, UserDto>
            .NewConfig()
            .Map( dest => dest.Id, src => src.Id )
            .Map( dest => dest.Login, src => src.Login )
            .Map( dest => dest.Name, src => src.Name )
            .Map( dest => dest.Description, src => src.Description )
            .Map( dest => dest.RecipeCount, src => src.RecipeCount );

        TypeAdapterConfig<UpdateUserDto, UpdateUserCommand>
            .NewConfig()
            .Ignore( dest => dest.Id )
            .Map( dest => dest.Name, src => src.Name )
            .Map( dest => dest.Description, src => src.Description )
            .Map( dest => dest.Login, src => src.Login )
            .Map( dest => dest.OldPassword, src => src.OldPassword )
            .Map( dest => dest.NewPassword, src => src.NewPassword );

        TypeAdapterConfig<RegisterUserDto, CreateUserCommand>
            .NewConfig()
            .Map( dest => dest.Name, src => src.Name )
            .Map( dest => dest.Password, src => src.Password )
            .Map( dest => dest.Login, src => src.Login );
    }
}
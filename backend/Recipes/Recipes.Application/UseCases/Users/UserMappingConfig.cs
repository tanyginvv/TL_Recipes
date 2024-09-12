
using Mapster;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Users;

public static class UserMappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<User, GetUserByIdQueryDto>.NewConfig()
           .Map( dest => dest.Id, src => src.Id )
           .Map( dest => dest.Name, src => src.Name )
           .Map( dest => dest.Description, src => src.Description )
           .Map( dest => dest.Login, src => src.Login )
           .Map( dest => dest.RecipeCount, src => src.Recipes.Count )
           .Map( dest => dest.LikeCount, src => src.Likes.Count )
           .Map( dest => dest.FavouriteCount, src => src.Favourites.Count );

        TypeAdapterConfig<User, GetUserNameByIdQueryDto>.NewConfig()
            .Map( dest => dest.Name, src => src.Name );
    }
}

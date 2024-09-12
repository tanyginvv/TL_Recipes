using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Users.Commands.CreateUser;
using Recipes.Application.UseCases.Users.Commands.UpdateUser;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.Application.UseCases.Users.Queries.GetUserById;
using Recipes.Application.UseCases.Users.Queries.GetUserNameById;

namespace Recipes.Application.UseCases.Users;

public static class UserBinding
{
    public static IServiceCollection AddUserBindings( this IServiceCollection services )
    {
        services.AddScoped<ICommandHandler<CreateUserCommand>, CreateUserCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand>, UpdateUserCommandHandler>();

        services.AddScoped<IQueryHandler<GetUserNameByIdQueryDto, GetUserNameByIdQuery>, GetUserNameByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetUserByIdQueryDto, GetUserByIdQuery>, GetUserByIdQueryHandler>();

        services.AddScoped<IAsyncValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddScoped<IAsyncValidator<UpdateUserCommand>, UpdateUserCommandValidator>();

        services.AddScoped<IAsyncValidator<GetUserNameByIdQuery>, GetUserNameByIdQueryValidator>();
        services.AddScoped<IAsyncValidator<GetUserByIdQuery>, GetUserByIdQueryValidator>();

        UserMappingConfig.RegisterMappings();
        return services;
    }
}
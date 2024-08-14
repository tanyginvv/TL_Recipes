using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetUserById;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.UseCases.Services.AuthService;
using Recipes.Application.UseCases.Users.Commands;
using Recipes.Application.UseCases.Users.Commands.DeleteUser;
using Recipes.Application.UseCases.Users.Commands.UpdateUser;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.Application.UseCases.Users.Queries.GetUserById;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Users;

public static class UserBinding
{
    public static IServiceCollection AddUserBindings( this IServiceCollection services )
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.AddScoped<ICommandHandler<CreateUserCommand>, CreateUserCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteUserCommand>, DeleteUserCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand>, UpdateUserCommandHandler>();

        services.AddScoped<IQueryHandler<GetUserLoginByIdQueryDto, GetUserLoginByIdQuery>, GetUserLoginByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetUserByIdQueryDto, GetUserByIdQuery>, GetUserByIdQueryHandler>();

        services.AddScoped<IAsyncValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddScoped<IAsyncValidator<DeleteUserCommand>, DeleteUserCommandValidator>();
        services.AddScoped<IAsyncValidator<UpdateUserCommand>, UpdateUserCommandValidator>();

        services.AddScoped<IAsyncValidator<GetUserLoginByIdQuery>, GetUserLoginByIdQueryValidator>();
        services.AddScoped<IAsyncValidator<GetUserByIdQuery>, GetUserByIdQueryValidator>();

        return services;
    }
}
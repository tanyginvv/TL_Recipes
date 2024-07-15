using Application.CQRSInterfaces;
using Application.Validation;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Steps.Commands.DeleteStepCommand;
using Recipes.Application.Steps.Commands.UpdateStepCommand;
using Recipes.Application.Steps.Dtos;
using Recipes.Application.Steps.Queries;

namespace Recipes.Application.Steps
{
    public static class StepBindings
    {
        public static IServiceCollection AddStepsBindings( this IServiceCollection services )
        {
            services.AddScoped<ICommandHandler<CreateStepCommand>, CreateStepCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateStepCommand>, UpdateStepCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteStepCommand>, DeleteStepCommandHandler>();

            services.AddScoped<IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQuery>, GetStepsByRecipeIdQueryHandler>();

            services.AddScoped<IAsyncValidator<CreateStepCommand>, CreateStepCommandValidator>();
            services.AddScoped<IAsyncValidator<DeleteStepCommand>, DeleteStepCommandValidator>();
            services.AddScoped<IAsyncValidator<UpdateStepCommand>, UpdateStepCommandValidator>();

            services.AddScoped<IAsyncValidator<GetStepsByRecipeIdQuery>, GetStepsByRecipeIdQueryValidator>();

            return services;
        }
    }
}
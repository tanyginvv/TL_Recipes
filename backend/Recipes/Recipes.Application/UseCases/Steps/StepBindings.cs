using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Steps.Commands;
using Recipes.Application.UseCases.Steps.Dtos;
using Recipes.Application.UseCases.Steps.Queries.GetStepsByRecipeIdQuery;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps
{
    public static class StepBindings
    {
        public static IServiceCollection AddStepsBindings( this IServiceCollection services )
        {
            services.AddScoped<ICommandHandlerWithResult<CreateStepCommand, Step>, CreateStepCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateStepCommand>, UpdateStepCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteStepCommand>, DeleteStepCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateStepsCommand>, UpdateStepsCommandHandler>();

            services.AddScoped<IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQuery>, GetStepsByRecipeIdQueryHandler>();

            services.AddScoped<IAsyncValidator<CreateStepCommand>, CreateStepCommandValidator>();
            services.AddScoped<IAsyncValidator<DeleteStepCommand>, DeleteStepCommandValidator>();
            services.AddScoped<IAsyncValidator<UpdateStepCommand>, UpdateStepCommandValidator>();
            services.AddScoped<IAsyncValidator<UpdateStepsCommand>, UpdateStepsCommandValidator>();

            services.AddScoped<IAsyncValidator<GetStepsByRecipeIdQuery>, GetStepsByRecipeIdQueryValidator>();

            return services;
        }
    }
}
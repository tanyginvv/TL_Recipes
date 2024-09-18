using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Steps.Commands.CreateStep;
using Recipes.Application.UseCases.Steps.Commands.DeleteStep;
using Recipes.Application.UseCases.Steps.Commands.UpdateStep;
using Recipes.Application.UseCases.Steps.Commands.UpdateSteps;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps;

public static class StepBindings
{
    public static IServiceCollection AddStepsBindings( this IServiceCollection services )
    {
        services.AddScoped<ICommandHandlerWithResult<CreateStepCommand, Step>, CreateStepCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateStepCommand>, UpdateStepCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteStepCommand>, DeleteStepCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateStepsCommand>, UpdateStepsCommandHandler>();


        services.AddScoped<IAsyncValidator<CreateStepCommand>, CreateStepCommandValidator>();
        services.AddScoped<IAsyncValidator<DeleteStepCommand>, DeleteStepCommandValidator>();
        services.AddScoped<IAsyncValidator<UpdateStepCommand>, UpdateStepCommandValidator>();
        services.AddScoped<IAsyncValidator<UpdateStepsCommand>, UpdateStepsCommandValidator>();


        StepMappingConfig.RegisterMappings();

        return services;
    }
}
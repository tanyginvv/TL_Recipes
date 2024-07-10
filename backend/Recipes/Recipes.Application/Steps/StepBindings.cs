using Application.CQRSInterfaces;
using Application.Validation;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Recipes.Commands.CreateRecipe;
using Recipes.Application.Recipes.Commands.DeleteRecipe;
using Recipes.Application.Recipes.Commands.UpdateRecipe;
using Recipes.Application.Recipes.Dtos;
using Recipes.Application.Recipes.Queries.GetAllRecipes;
using Recipes.Application.Recipes.Queries.GetRecipe;
using Recipes.Application.Recipes.Queries.GetRecipeById;
using Recipes.Application.Steps.Commands;
using Recipes.Application.Steps.Commands.CreateStepCommand;
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

            services.AddScoped<IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQueryDto>, GetStepsByRecipeIdQueryHandler>();

            services.AddScoped<IAsyncValidator<CreateStepCommandDto>, CreateStepCommandValidator>();
            services.AddScoped<IAsyncValidator<DeleteStepCommand>, DeleteStepCommandValidator>();
            services.AddScoped<IAsyncValidator<UpdateStepCommand>, UpdateStepCommandValidator>();

            services.AddScoped<IAsyncValidator<GetStepsByRecipeIdQueryDto>, GetStepsByRecipeIdQueryValidator>();

            return services;
        }
    }
}
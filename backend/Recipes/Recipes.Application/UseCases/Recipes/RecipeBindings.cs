using Recipes.Application.CQRSInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Validation;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Recipes.Commands.CreateRecipe;
using Recipes.Application.UseCases.Recipes.Commands.DeleteRecipe;
using Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe;
using Recipes.Application.UseCases.Recipes.Commands.UpdateRecipeTags;
using Recipes.Application.UseCases.Recipes.Queries.GetAllRecipes;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipeById;

namespace Recipes.Application.UseCases.Recipes
{
    public static class RecipesBindings
    {
        public static IServiceCollection AddRecipesBindings( this IServiceCollection services )
        {
            services.AddScoped<ICommandHandler<CreateRecipeCommand>, CreateRecipeCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateRecipeCommand>, UpdateRecipeCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteRecipeCommand>, DeleteRecipeCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateRecipeTagsCommand>, UpdateRecipeTagsCommandHandler>();

            services.AddScoped<IQueryHandler<GetRecipeByIdQueryDto, GetRecipeByIdQuery>, GetRecipeByIdQueryHandler>();
            services.AddScoped<IQueryHandler<IEnumerable<RecipeDto>, GetAllRecipesQuery>, GetAllRecipesQueryHandler>();

            services.AddScoped<IAsyncValidator<CreateRecipeCommand>, CreateRecipeCommandValidator>();
            services.AddScoped<IAsyncValidator<DeleteRecipeCommand>, DeleteRecipeCommandValidator>();
            services.AddScoped<IAsyncValidator<UpdateRecipeCommand>, UpdateRecipeCommandValidator>();
            services.AddScoped<IAsyncValidator<UpdateRecipeTagsCommand>, UpdateRecipeTagsCommandValidator>();

            services.AddScoped<IAsyncValidator<GetRecipeByIdQuery>, GetRecipeByIdQueryValidator>();
            services.AddScoped<IAsyncValidator<GetAllRecipesQuery>, GetAllRecipesQueryValidator>();

            return services;
        }
    }
}
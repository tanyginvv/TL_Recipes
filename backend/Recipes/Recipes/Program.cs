using Application.Repositories;
using Recipes.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Recipes.Infrastructure.Entities.Recipes;
using Recipes.Application.Recipes;
using Recipes.Application.Tags;
using Recipes.Application.Ingredients;
using Recipes.Application.Steps;
using Application;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddDbContext<RecipesDbContext>( options =>
    options.UseSqlServer( builder.Configuration.GetConnectionString( "Recipes" ) ) );

// Регистрация репозитория
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

// Регистрация других зависимостей
builder.Services.AddControllers();
builder.Services.AddRecipesBindings();
builder.Services.AddTagsBindings();
builder.Services.AddIngredientsBindings();
builder.Services.AddStepsBindings();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

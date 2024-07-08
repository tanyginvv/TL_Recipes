using Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Recipes.Infrastructure.Entities.Recipes;
using Recipes.Infrastructure.Context;

var builder = WebApplication.CreateBuilder( args );

// Add services to the DI-container.
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

builder.Services.AddDbContext<RecipesDbContext>( options =>
    options.UseSqlServer( builder.Configuration.GetConnectionString( "Recipes" ) ) );

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

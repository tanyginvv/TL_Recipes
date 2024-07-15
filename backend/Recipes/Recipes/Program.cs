using Recipes.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Recipes.Application;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddDbContext<RecipesDbContext>( options =>
    options.UseSqlServer( builder.Configuration.GetConnectionString( "Recipes" ) ) );

// Регистрация других зависимостей
builder.Services.AddControllers();
builder.Services.AddApplicationBindings();
builder.Services.AddInfrastructureBindings();

// Добавление CORS
builder.Services.AddCors( options =>
{
    options.AddPolicy( "AllowSpecificOrigin",
        builder => builder.WithOrigins( "http://localhost:5173" ) // Замените на URL вашего фронтенда
                          .AllowAnyHeader()
                          .AllowAnyMethod() );
} );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Использование CORS
app.UseCors( "AllowSpecificOrigin" );

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

using Recipes.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Recipes.Application;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddDbContext<RecipesDbContext>( options =>
    options.UseSqlServer( builder.Configuration.GetConnectionString( "Recipes" ) ) );

// ����������� ������ ������������
builder.Services.AddControllers();
builder.Services.AddApplicationBindings();
builder.Services.AddInfrastructureBindings();

// ���������� CORS
builder.Services.AddCors( options =>
{
    options.AddPolicy( "AllowSpecificOrigin",
        builder => builder.WithOrigins( "http://localhost:5173" ) // �������� �� URL ������ ���������
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

// ������������� CORS
app.UseCors( "AllowSpecificOrigin" );

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

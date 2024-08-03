using Recipes.Application;
using Recipes.Infrastructure;

var builder = WebApplication.CreateBuilder( args );

IConfiguration configuration = new ConfigurationBuilder()
              .AddJsonFile( "appsettings.json" )
              .AddJsonFile( $"appsettings.{builder.Environment.EnvironmentName}.json" )
              .Build();

builder.Services.AddApplicationBindings();
builder.Services.AddInfrastructureBindings( builder.Configuration );
builder.Services.AddControllers();

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
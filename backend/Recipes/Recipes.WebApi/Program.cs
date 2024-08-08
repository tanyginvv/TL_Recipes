using Recipes.Application;
using Recipes.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

builder.Configuration
    .AddJsonFile( "appsettings.json" )
    .AddJsonFile( $"appsettings.{Environment.GetEnvironmentVariable( "JSON_CONFIG_NAME" )}.json" )
    .Build();

builder.Services.AddApplicationBindings();
builder.Services.AddInfrastructureBindings( builder.Configuration );
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
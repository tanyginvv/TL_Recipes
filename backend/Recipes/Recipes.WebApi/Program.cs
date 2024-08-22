using Recipes.Application;
using Recipes.Infrastructure;
using Recipes.Infrastructure.ConfigurationUtils;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

string environmentName = Environment.GetEnvironmentVariable( "JSON_CONFIG_NAME" ) ?? "Development";
builder.Configuration
       .AddJsonFile( "appsettings.json" )
       .AddJsonFile( $"appsettings.{environmentName}.json", optional: true )
       .Build();

builder.Services.Configure<JwtOptions>( builder.Configuration.GetSection( "JWTOptions" ) );

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
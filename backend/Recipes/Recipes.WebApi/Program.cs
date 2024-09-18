using Recipes.Application;
using Recipes.Application.Options;
using Recipes.Infrastructure;
using Recipes.WebApi.Extensions;
using Serilog;
using Serilog.Events;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

string environmentName = Environment.GetEnvironmentVariable( "JSON_CONFIG_NAME" ) ?? "dev";
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration( builder.Configuration )
    .WriteTo.Console( outputTemplate: LogConfig.LogFormat )
    .WriteTo.File( "logs/errors/log-.txt", outputTemplate: LogConfig.LogFormat, restrictedToMinimumLevel: LogEventLevel.Warning, rollingInterval: RollingInterval.Day )
    .WriteTo.File( "logs/info/log-.txt", outputTemplate: LogConfig.LogFormat, rollingInterval: RollingInterval.Day )
    .CreateLogger();

builder.Host.UseSerilog();

builder.Configuration
       .AddJsonFile( "appsettings.json" )
       .AddJsonFile( $"appsettings.{environmentName}.json", optional: true )
       .Build();

builder.Services.Configure<JwtOptions>( builder.Configuration.GetSection( "JWTOptions" ) );
builder.Services.Configure<FileToolsOptions>( builder.Configuration.GetSection( "FileToolsOptions" ) );

builder.Services.AddApplicationBindings();
builder.Services.AddInfrastructureBindings( builder.Configuration );
builder.Services.AddControllers();

builder.Services.AddCors( options =>
{
    options.AddPolicy( "AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins( builder.Configuration.GetSection( "FrontendUrl" ).Value )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    } );
} );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors( "AllowSpecificOrigin" );
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
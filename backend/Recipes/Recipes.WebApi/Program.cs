using Recipes.Application;
using Recipes.Application.Options;
using Recipes.Infrastructure;
using Serilog;
using Microsoft.Extensions.FileProviders;
using Recipes.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

string environmentName = Environment.GetEnvironmentVariable( "JSON_CONFIG_NAME" ) ?? "dev";
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration( builder.Configuration )
    .WriteTo.Console()
    .WriteTo.File( "logs/log-.txt", rollingInterval: RollingInterval.Day )
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

WebApplication app = builder.Build();

using ( IServiceScope scope = app.Services.CreateScope() )
{
    IServiceProvider services = scope.ServiceProvider;
    try
    {
        RecipesDbContext context = services.GetRequiredService<RecipesDbContext>();
        context.Database.Migrate(); 
    }
    catch ( Exception ex )
    {
        ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError( ex, "An error occurred while migrating the database." );
    }
}

app.UseCors( "AllowSpecificOrigin" );

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseFileServer( new FileServerOptions
{
    FileProvider = new PhysicalFileProvider( Path.Combine( app.Environment.ContentRootPath, "wwwroot" ) ),
    RequestPath = "",
    EnableDirectoryBrowsing = false
} );

app.Use( async ( context, next ) =>
{
    await next();
    if ( context.Response.StatusCode == 404 && !context.Request.Path.Value.StartsWith( "/api" ) )
    {
        context.Request.Path = "/index.html";
        await next();
    }
} );


app.MapFallbackToFile( "index.html" );

app.UseAuthorization();
app.MapControllers();

app.Run();
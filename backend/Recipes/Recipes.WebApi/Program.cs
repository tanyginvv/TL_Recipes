using Recipes.Application;
using Recipes.Application.Options;
using Recipes.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

string environmentName = Environment.GetEnvironmentVariable( "JSON_CONFIG_NAME" ) ?? "dev";
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
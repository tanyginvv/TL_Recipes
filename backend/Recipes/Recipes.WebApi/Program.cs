using Microsoft.AspNetCore.CookiePolicy;
using Recipes.Application;
using Recipes.Infrastructure;
using Recipes.WebApi;

var builder = WebApplication.CreateBuilder( args );

IConfiguration configuration = new ConfigurationBuilder()
              .AddJsonFile( "appsettings.json" )
              .AddJsonFile( $"appsettings.{builder.Environment.EnvironmentName}.json" )
              .Build();

builder.Services.AddApplicationBindings();
builder.Services.AddInfrastructureBindings( builder.Configuration );
builder.Services.AddControllers();

builder.Services.AddCors( options =>
{
    options.AddPolicy( "AllowSpecificOrigin",
        builder => builder.WithOrigins( "http://localhost:5173" )
                          .AllowAnyHeader()
                          .AllowAnyMethod() );
} );

// Ensure API authentication uses the TokenConfiguration
builder.Services.AddApiAuthentication();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCookiePolicy( new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
} );

app.UseCors( "AllowSpecificOrigin" );
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
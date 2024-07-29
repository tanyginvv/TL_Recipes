using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Infrastructure.ConfigurationUtils.Token;
using Recipes.Application;
using Recipes.Infrastructure;
using Recipes.WebApi;
using Recipes.Application.Tokens;

var builder = WebApplication.CreateBuilder( args );

// Register dependencies
builder.Services.AddAutoMapper( typeof( Program ).Assembly );
builder.Services.AddApplicationBindings();
builder.Services.AddInfrastructureBindings( builder.Configuration );
builder.Services.AddControllers();

// Register token configuration
builder.Services.Configure<TokenConfiguration>( builder.Configuration.GetSection( "JWTOptions" ) );
builder.Services.AddSingleton<ITokenConfiguration>( provider =>
{
    var options = provider.GetRequiredService<IOptions<TokenConfiguration>>().Value;
    return options;
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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

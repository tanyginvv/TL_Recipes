using Infrastructure.ConfigurationUtils.Token;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Application;
using Recipes.Application.Tokens;
using Recipes.Infrastructure;
using Recipes.WebApi;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder( args );

// Register dependencies
builder.Services.AddAutoMapper( typeof( Program ).Assembly );
builder.Services.AddApplicationBindings();
builder.Services.AddInfrastructureBindings( builder.Configuration );
builder.Services.AddControllers();


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
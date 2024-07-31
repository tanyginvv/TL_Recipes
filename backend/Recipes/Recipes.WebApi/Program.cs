using Microsoft.AspNetCore.CookiePolicy;
using Recipes.Application;
using Recipes.Infrastructure;
using Recipes.WebApi;

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
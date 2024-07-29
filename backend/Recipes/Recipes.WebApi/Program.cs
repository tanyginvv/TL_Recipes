using Infrastructure.ConfigurationUtils.Token;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Options;
using Recipes.Application;
using Recipes.Application.Tokens;
using Recipes.Infrastructure;
using Recipes.WebApi;
var builder = WebApplication.CreateBuilder( args );

builder.Services.AddAutoMapper( typeof( Program ).Assembly );
builder.Services.AddApplicationBindings();
builder.Services.AddInfrastructureBindings( builder.Configuration );
builder.Services.AddControllers();

builder.Services.Configure<TokenConfiguration>( builder.Configuration.GetSection( "TokenConfiguration" ) );
builder.Services.AddSingleton<ITokenConfiguration>( provider => provider.GetRequiredService<IOptions<TokenConfiguration>>().Value );

builder.Services.AddApiAuthentication( builder.Configuration.GetSection( "TokenConfiguration" ).Get<ITokenConfiguration>() );

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

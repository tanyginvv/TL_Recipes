namespace Recipes.WebApi.JwtAuthorization
{
    public static class JwtAuthorizationBindings
    {
        public static IServiceCollection AddJwtAuthBindings( this IServiceCollection services )
        {
            services.AddScoped<JwtAuthorizationAttribute>();

            services.AddScoped<ILogger<JwtAuthorizationAttribute>, Logger<JwtAuthorizationAttribute>>();

            return services;
        }
    }
}
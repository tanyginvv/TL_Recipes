namespace Recipes.WebApi.Extensions;
public static class LogConfig
{
    public const string LogFormat =
        "{Timestamp:HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}";
}
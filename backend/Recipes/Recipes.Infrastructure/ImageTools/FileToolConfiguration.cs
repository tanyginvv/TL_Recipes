using Microsoft.Extensions.Options;
using Recipes.Application.Interfaces;

namespace Recipes.Infrastructure.ImageTools;

public class FileToolConfiguration( IOptions<FileToolsOptions> toolOptions) : IFileToolConfiguration
{
    public string GetStorageUrl()
    {
        return toolOptions.Value.StorageUrl;
    }
}
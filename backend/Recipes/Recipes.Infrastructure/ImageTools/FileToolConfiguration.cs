using Microsoft.Extensions.Options;
using Recipes.Infrastructure.Options;

namespace Recipes.Infrastructure.ImageTools;

public class FileToolConfiguration( IOptions<FileToolsOptions> toolOptions)
{
    public string GetStorageUrl()
    {
        return toolOptions.Value.StorageUrl;
    }
}
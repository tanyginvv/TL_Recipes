using Recipes.Application.Results;

namespace Recipes.Application.Interfaces;

public interface IImageTools
{
    Task<Result<string>> SaveImageAsync( IFile image );
    Result<byte[]> GetImage( string imageName );
    Result<bool> DeleteImage( string imageName );
}
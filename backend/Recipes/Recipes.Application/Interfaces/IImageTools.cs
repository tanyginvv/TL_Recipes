namespace Recipes.Application.Interfaces;

public interface IImageTools
{
    Task<string> SaveImageAsync( IFile image );
    byte[] GetImage( string imageName );
    bool DeleteImage( string imageName );
}
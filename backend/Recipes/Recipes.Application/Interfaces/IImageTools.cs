using Microsoft.AspNetCore.Http;

namespace Recipes.Application.Interfaces
{
    public interface IImageTools
    {
        Task<string> SaveImageAsync( IFormFile image );
        byte[] GetImage( string imageName );
        bool DeleteImage( string imageName );
    }
}
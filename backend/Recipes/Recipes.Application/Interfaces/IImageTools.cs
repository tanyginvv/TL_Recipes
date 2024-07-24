using Microsoft.AspNetCore.Http;

namespace Recipes.Application.Interfaces
{
    public interface IImageTools
    {
        Task<string> SaveRecipeImageAsync( IFormFile image );
        byte[] GetImage( string imageName );
    }
}
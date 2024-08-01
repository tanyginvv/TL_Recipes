using Microsoft.AspNetCore.Http;
using Recipes.Application.Interfaces;

namespace Recipes.Infrastructure.ImageTools
{
    public class ImageHelperTools : IImageTools
    {
        private const string StoreUrl = "../Recipes.Infrastructure/store";

        public async Task<string> SaveRecipeImageAsync( IFormFile image )
        {
            if ( image is null )
            {
                return null;
            }

            string currentDirectory = Directory.GetCurrentDirectory();
            string folderPath = Path.Combine( currentDirectory, StoreUrl );
            string fileName = Guid.NewGuid() + Path.GetExtension( image.FileName );
            string filePath = Path.Combine( folderPath, fileName );

            if ( !Directory.Exists( folderPath ) )
            {
                Directory.CreateDirectory( folderPath );
            }

            if ( image.Length > 0 )
            {
                using ( FileStream stream = new FileStream( filePath, FileMode.Create ) )
                {
                    await image.CopyToAsync( stream );
                }
            }

            return fileName;
        }

        public byte[] GetImage( string imageName )
        {
            if ( string.IsNullOrEmpty( imageName ) )
            {
                return null;
            }

            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string folderPath = Path.Combine( currentDirectory, StoreUrl );
                string filePath = Path.Combine( folderPath, imageName );

                if ( File.Exists( filePath ) )
                {
                    return File.ReadAllBytes( filePath );
                }

                return null;
            }
            catch ( Exception )
            {
                return null;
            }
        }

        public bool DeleteImage( string imageName )
        {
            if ( string.IsNullOrEmpty( imageName ) )
            {
                return false;
            }

            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string folderPath = Path.Combine( currentDirectory, StoreUrl );
                string filePath = Path.Combine( folderPath, imageName );

                if ( File.Exists( filePath ) )
                {
                    File.Delete( filePath );
                    return true;
                }

                return false;
            }
            catch ( Exception )
            {
                return false;
            }
        }
    }
}
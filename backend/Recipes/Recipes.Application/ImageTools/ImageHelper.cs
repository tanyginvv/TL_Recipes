using Microsoft.AspNetCore.Http;

namespace Recipes.Application.ImageTools
{
    public class ImageHelperTools
    {
        private const string STORE_URL = "../Recipes.Infrastructure/store";

        public async Task<string> SaveRecipeImageAsync( IFormFile image )
        {
            if ( image is null )
            {
                return null;
            }

            string currentDirectory = Directory.GetCurrentDirectory();
            string folderPath = Path.Combine( currentDirectory, STORE_URL );
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

        public static byte[] GetImage( string imageName )
        {
            if ( string.IsNullOrEmpty( imageName ) )
            {
                return null;
            }

            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string folderPath = Path.Combine( currentDirectory, STORE_URL );
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
    }
}
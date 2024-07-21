using Microsoft.AspNetCore.Http;

namespace Recipes.Application.ImageTools
{
    public class ImageHelperTools
    {
        public async Task<string> SaveRecipeImageAsync( IFormFile image )
        {
            if ( image == null )
            {
                return null;
            }

            var currentDirectory = Directory.GetCurrentDirectory();
            var folderPath = Path.Combine( currentDirectory, "../Recipes.Infrastructure/store" );
            var fileName = Guid.NewGuid() + Path.GetExtension( image.FileName );
            var filePath = Path.Combine( folderPath, fileName );

            if ( !Directory.Exists( folderPath ) )
            {
                Directory.CreateDirectory( folderPath );
            }

            if ( image.Length > 0 )
            {
                using ( var stream = new FileStream( filePath, FileMode.Create ) )
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
                var currentDirectory = Directory.GetCurrentDirectory();
                var folderPath = Path.Combine( currentDirectory, "../Recipes.Infrastructure/store" );
                var filePath = Path.Combine( folderPath, imageName );

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
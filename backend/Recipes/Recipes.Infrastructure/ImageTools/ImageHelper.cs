using Recipes.Application.Interfaces;

namespace Recipes.Infrastructure.ImageTools
{
    public class ImageHelperTools : IImageTools
    {
        private const string StorageUrl = "../Recipes.Infrastructure/Storage";

        public async Task<string> SaveImageAsync( IFile file )
        {
            if ( file is null || file.Length == 0 )
            {
                return null;
            }

            string currentDirectory = Directory.GetCurrentDirectory();
            string folderPath = Path.Combine( currentDirectory, StorageUrl );
            string fileName = Guid.NewGuid() + Path.GetExtension( file.FileName );
            string filePath = Path.Combine( folderPath, fileName );

            if ( !Directory.Exists( folderPath ) )
            {
                Directory.CreateDirectory( folderPath );
            }

            using ( FileStream stream = new FileStream( filePath, FileMode.Create ) )
            {
                await file.OpenReadStream().CopyToAsync( stream );
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
                string folderPath = Path.Combine( currentDirectory, StorageUrl );
                string filePath = Path.Combine( folderPath, imageName );

                return File.Exists( filePath ) ? File.ReadAllBytes( filePath ) : null;
            }
            catch
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
                string folderPath = Path.Combine( currentDirectory, StorageUrl );
                string filePath = Path.Combine( folderPath, imageName );

                if ( File.Exists( filePath ) )
                {
                    File.Delete( filePath );
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Recipes.Application.Interfaces;
using Recipes.Application.Options;
using Recipes.Application.Results;

namespace Recipes.Infrastructure.ImageTools;

public class FileImageTools( IOptions<FileToolsOptions> fileToolsOptions, ILogger<FileImageTools> logger ) : IImageTools
{
    public async Task<Result<string>> SaveImageAsync( IFile file )
    {
        if ( file is null || file.Length == 0 )
        {
            logger.LogWarning( "SaveImageAsync: File is null or empty." );
            return Result<string>.FromError( "Файл не предоставлен или пуст." );
        }

        try
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string folderPath = Path.Combine( currentDirectory, fileToolsOptions.Value.StorageUrl );
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

            logger.LogInformation( $"SaveImageAsync: Successfully saved file {fileName}." );
            return Result<string>.FromSuccess( fileName );
        }
        catch ( Exception ex )
        {
            logger.LogError( ex, "SaveImageAsync: Failed to save image." );
            return Result<string>.FromError( "Не удалось сохранить изображение." );
        }
    }

    public Result<byte[]> GetImage( string imageName )
    {
        if ( string.IsNullOrEmpty( imageName ) )
        {
            logger.LogWarning( "GetImage: Image name is not provided." );
            return Result<byte[]>.FromError( "Имя файла не указано." );
        }

        try
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string folderPath = Path.Combine( currentDirectory, fileToolsOptions.Value.StorageUrl );
            string filePath = Path.Combine( folderPath, imageName );

            if ( File.Exists( filePath ) )
            {
                byte[] imageBytes = File.ReadAllBytes( filePath );
                logger.LogInformation( $"GetImage: Successfully read file {imageName}." );
                return Result<byte[]>.FromSuccess( imageBytes );
            }
            else
            {
                logger.LogWarning( $"GetImage: File {imageName} not found." );
                return Result<byte[]>.FromError( "Файл не найден." );
            }
        }
        catch ( Exception ex )
        {
            logger.LogError( ex, "GetImage: Failed to read image." );
            return Result<byte[]>.FromError( "Не удалось прочитать изображение." );
        }
    }

    public Result<bool> DeleteImage( string imageName )
    {
        if ( string.IsNullOrEmpty( imageName ) )
        {
            logger.LogWarning( "DeleteImage: Image name is not provided." );
            return Result<bool>.FromError( "Имя файла не указано." );
        }

        try
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string folderPath = Path.Combine( currentDirectory, fileToolsOptions.Value.StorageUrl );
            string filePath = Path.Combine( folderPath, imageName );

            if ( File.Exists( filePath ) )
            {
                File.Delete( filePath );
                logger.LogInformation( $"DeleteImage: Successfully deleted file {imageName}." );
                return Result<bool>.FromSuccess( true );
            }
            else
            {
                logger.LogWarning( $"DeleteImage: File {imageName} not found." );
                return Result<bool>.FromError( "Файл не найден." );
            }
        }
        catch ( Exception ex )
        {
            logger.LogError( ex, "DeleteImage: Failed to delete image." );
            return Result<bool>.FromError( "Не удалось удалить изображение." );
        }
    }
}
using Microsoft.Extensions.Options;
using Recipes.Application.Interfaces;
using Recipes.Application.Options;
using Recipes.Application.Results;
using Serilog;

namespace Recipes.Infrastructure.ImageTools;

public class FileImageTools( IOptions<FileToolsOptions> fileToolsOptions ) : IImageTools
{
    public async Task<Result<string>> SaveImageAsync( IFile file )
    {
        if ( file is null || file.Length == 0 )
        {
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

            return Result<string>.FromSuccess( fileName );
        }
        catch ( Exception ex )
        {
            Log.Error( ex, "Произошла ошибка." );

            return Result<string>.FromError( "Не удалось сохранить изображение." );
        }
    }

    public Result<byte[]> GetImage( string imageName )
    {
        if ( string.IsNullOrEmpty( imageName ) )
        {
            return Result<byte[]>.FromError( "Имя файла не указано." );
        }

        try
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string folderPath = Path.Combine( currentDirectory, fileToolsOptions.Value.StorageUrl );
            string filePath = Path.Combine( folderPath, imageName );

            if ( File.Exists( filePath ) )
            {
                return Result<byte[]>.FromSuccess( File.ReadAllBytes( filePath ) );
            }
            else
            {
                return Result<byte[]>.FromError( "Файл не найден." );
            }
        }
        catch
        {
            return Result<byte[]>.FromError( "Не удалось прочитать изображение." );
        }
    }

    public Result<bool> DeleteImage( string imageName )
    {
        if ( string.IsNullOrEmpty( imageName ) )
        {
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
                return Result<bool>.FromSuccess( true );
            }
            else
            {
                return Result<bool>.FromError( "Файл не найден." );
            }
        }
        catch
        {
            return Result<bool>.FromError( "Не удалось удалить изображение." );
        }
    }
}
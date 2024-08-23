using Microsoft.AspNetCore.Mvc;
using Recipes.Application.Interfaces;
using Recipes.Infrastructure.ImageTools;
using Recipes.WebApi.JwtAuthorization;

namespace Recipes.WebApi.Controllers;

[ApiController]
[Route( "api/images" )]
public class ImagesController : ControllerBase
{
    [JwtAuthorization]
    [HttpPost( "upload" )]
    public async Task<IActionResult> UploadImage(
        IFormFile image,
        [FromServices] IImageTools imageHelperTools )
    {
        if ( image is null )
        {
            return BadRequest( "Изображение не предоставлено" );
        }

        IFile file = new FormFileAdapter( image );
        string fileName = await imageHelperTools.SaveImageAsync( file );

        if ( string.IsNullOrEmpty( fileName ) )
        {
            return StatusCode( 500, "Ошибка сохранения картинки" );
        }

        return Ok( new { FileName = fileName } );
    }

    [HttpGet( "{fileName}" )]
    public IActionResult GetImage(
        [FromRoute] string fileName,
        [FromServices] IImageTools imageHelperTools )
    {
        byte[] imageBytes = imageHelperTools.GetImage( fileName );

        if ( imageBytes == null )
        {
            return NotFound( "Картинка не найдена" );
        }

        return File( imageBytes, "image/jpeg" );
    }

    [JwtAuthorization]
    [HttpDelete( "{fileName}" )]
    public IActionResult DeleteImage(
        [FromRoute] string fileName,
        [FromServices] IImageTools imageHelperTools )
    {
        bool imageDeleted = imageHelperTools.DeleteImage( fileName );

        if ( !imageDeleted )
        {
            return NotFound( "Картинка не найдена" );
        }

        return Ok( "Картинка успешно удалена" );
    }
}
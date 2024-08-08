using Microsoft.AspNetCore.Mvc;
using Recipes.Application.Interfaces;

namespace Recipes.WebApi.Controllers;

[ApiController]
[Route( "api/images" )]
public class ImagesController(): ControllerBase
{
    [HttpPost( "upload" )]
    public async Task<IActionResult> UploadImage( IFormFile image,
        [FromServices] IImageTools imageHelperTools )
    {
        if ( image is null )
        {
            return BadRequest( "Изображение не предоставлено" );
        }

        string fileName = await imageHelperTools.SaveImageAsync( image );

        if ( string.IsNullOrEmpty( fileName ) )
        {
            return StatusCode( 500, "Ошибка сохранения картинки" );
        }

        return Ok( new { FileName = fileName } );
    }

    [HttpGet( "{fileName}" )]
    public IActionResult GetImage( [FromRoute] string fileName,
        [FromServices] IImageTools imageHelperTools )
    {
        byte[] imageBytes = imageHelperTools.GetImage( fileName );

        if ( imageBytes is null )
        {
            return NotFound( "Картинка не найдена" );
        }

        return File( imageBytes, "image/jpeg" );
    }

    [HttpDelete( "{fileName}" )]
    public IActionResult DeleteImage( [FromRoute] string fileName,
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
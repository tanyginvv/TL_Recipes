using Microsoft.AspNetCore.Mvc;
using Recipes.Application.Interfaces;

namespace Recipes.WebApi.Controllers
{
    [ApiController]
    [Route( "api/images" )]
    public class ImagesController( IImageTools imageHelperTools )
        : ControllerBase
    {
        [HttpPost( "upload" )]
        public async Task<IActionResult> UploadImage( IFormFile image )
        {
            if ( image is null )
            {
                return BadRequest( "Изображение не предоставлено" );
            }

            string fileName = await imageHelperTools.SaveRecipeImageAsync( image );

            if ( string.IsNullOrEmpty( fileName ) )
            {
                return StatusCode( 500, "Ошибка сохранения картинки" );
            }

            return Ok( new { FileName = fileName } );
        }

        [HttpGet( "{fileName}" )]
        public IActionResult GetImage( [FromRoute] string fileName )
        {
            byte[] imageBytes = imageHelperTools.GetImage( fileName );

            if ( imageBytes is null )
            {
                return NotFound( "Картинка не найдена" );
            }

            return File( imageBytes, "image/jpeg" );
        }

        [HttpDelete( "{fileName}" )]
        public IActionResult DeleteImage( [FromRoute] string fileName )
        {
            bool imageDeleted = imageHelperTools.DeleteImage( fileName );

            if ( !imageDeleted )
            {
                return NotFound( "Картинка не найдена" );
            }

            return Ok( "Картинка успешно удалена" );
        }
    }
}
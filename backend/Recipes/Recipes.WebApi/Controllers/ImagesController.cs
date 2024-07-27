using Microsoft.AspNetCore.Mvc;
using Recipes.Application.ImageTools;

namespace Recipes.WebApi.Controllers
{
    [ApiController]
    [Route( "api/images" )]
    public class ImagesController( ImageHelperTools imageHelperTools )
        : ControllerBase
    {
        [HttpPost( "upload" )]
        public async Task<IActionResult> UploadImage( IFormFile image )
        {
            if ( image == null )
            {
                return BadRequest( "No image provided" );
            }

            var fileName = await imageHelperTools.SaveRecipeImageAsync( image );

            if ( string.IsNullOrEmpty( fileName ) )
            {
                return StatusCode( 500, "Error saving the image" );
            }

            return Ok( new { FileName = fileName } );
        }

        [HttpGet( "{fileName}" )]
        public IActionResult GetImage( [FromRoute] string fileName )
        {
            var imageBytes = ImageHelperTools.GetImage( fileName );

            if ( imageBytes == null )
            {
                return NotFound( "Image not found" );
            }

            return File( imageBytes, "image/jpeg" );
        }
    }
}
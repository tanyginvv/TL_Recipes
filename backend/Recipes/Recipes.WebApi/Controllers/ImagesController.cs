using Microsoft.AspNetCore.Mvc;
using Recipes.Application.UseCases.Recipes.ImageHelper;

namespace Recipes.WebApi.Controllers
{
    [ApiController]
    [Route( "api/images" )]
    public class ImagesController : ControllerBase
    {
        private readonly ImageHelperTools _imageHelperTools;

        public ImagesController( ImageHelperTools imageHelperTools )
        {
            _imageHelperTools = imageHelperTools;
        }

        [HttpPost( "upload" )]
        public async Task<IActionResult> UploadImage( IFormFile image )
        {
            if ( image == null )
            {
                return BadRequest( "No image provided" );
            }

            var fileName = await _imageHelperTools.SaveRecipeImageAsync( image );

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

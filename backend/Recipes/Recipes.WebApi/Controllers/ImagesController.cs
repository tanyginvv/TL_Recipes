using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.Interfaces;
using Recipes.Application.Results;
using Recipes.WebApi.Adapters;
using Recipes.WebApi.JwtAuthorization;

namespace Recipes.WebApi.Controllers;

[ApiController]
[Route( "api/images" )]
public class ImagesController : ControllerBase
{
    [JwtAuthorization]
    [HttpPost( "upload" )]
    public async Task<ActionResult<Result<string>>> UploadImage(
        [NotNull] IFormFile image,
        [FromServices] IImageTools imageHelperTools )
    {
        if ( image is null )
        {
            return BadRequest( "Изображение не предоставлено" );
        }

        IFile file = new FormFileAdapter( image );
        Result<string> result = await imageHelperTools.SaveImageAsync( file );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok( new { FileName = result.Value } );
    }

    [HttpGet( "{fileName}" )]
    public ActionResult<Result> GetImage(
        [FromRoute] string fileName,
        [FromServices] IImageTools imageHelperTools )
    {
        Result<byte[]> result = imageHelperTools.GetImage( fileName );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return File( result.Value, "image/jpeg" );
    }

    [JwtAuthorization]
    [HttpDelete( "{fileName}" )]
    public ActionResult<Result> DeleteImage(
        [FromRoute] string fileName,
        [FromServices] IImageTools imageHelperTools )
    {
        Result<bool> result = imageHelperTools.DeleteImage( fileName );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok();
    }
}
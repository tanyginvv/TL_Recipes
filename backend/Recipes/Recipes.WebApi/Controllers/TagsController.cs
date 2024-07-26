using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Tags.Queries.GetTagsForSearch;
using Recipes.WebApi.Dto.TagDtos;

namespace Recipes.WebApi.Controllers
{
    [ApiController]
    [Route( "api/tags" )]
    public class TagsController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadTagDto>>> GetTagsForSearch(
            [FromServices] IQueryHandler<IReadOnlyList<TagDto>, GetTagsForSearchQuery> getTagsForSearchQueryHandler,
            [FromQuery] int count = 5 )
        {
            GetTagsForSearchQuery query = new() { Count = count };
            Result<IReadOnlyList<TagDto>> result = await getTagsForSearchQueryHandler.HandleAsync( query );

            if ( !result.IsSuccess )
            {
                return NotFound( result.Error );
            }

            return Ok( result.Value );
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Queries.GetRandomTags;
using Recipes.Domain.Entities;

namespace Recipes.WebApi.Controllers
{
    [ApiController]
    [Route( "api/tags" )]
    public class TagsController( IQueryHandler<IReadOnlyList<Tag>, GetTagsForSearchQuery> getTagsForSearchQueryHandler )
        : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Tag>>> GetTagsForSearch( [FromQuery] int count = 5 )
        {
            GetTagsForSearchQuery query = new() { Count = count };
            Result<IReadOnlyList<Tag>> result = await getTagsForSearchQueryHandler.HandleAsync( query );

            if ( !result.IsSuccess )
            {
                return NotFound( result.Error );
            }

            return Ok( result.Value );
        }
    }
}
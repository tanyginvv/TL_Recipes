using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Tags.Queries.GetRandomTags;
using Recipes.Domain.Entities;

namespace Recipes.WebApi.Controllers
{
    [ApiController]
    [Route( "api/tags" )]
    public class TagsController( IQueryHandler<IReadOnlyList<Tag>, GetRandomTagsQuery> getRandomTagsQueryHandler )
        : ControllerBase
    {
        private IQueryHandler<IReadOnlyList<Tag>, GetRandomTagsQuery> _getRandomTagsQueryHandler => getRandomTagsQueryHandler;

        [HttpGet]
        public async Task<IActionResult> GetDistinctTags( [FromQuery] int count = 5 )
        {
            var query = new GetRandomTagsQuery { Count = count };
            var result = await _getRandomTagsQueryHandler.HandleAsync( query );

            if ( !result.IsSuccess )
            {
                return NotFound( result.Error );
            }

            return Ok( result.Value );
        }
    }
}

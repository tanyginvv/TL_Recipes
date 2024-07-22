using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Commands.CreateRecipe;
using Recipes.Application.UseCases.Recipes.Commands.DeleteRecipe;
using Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipeById;
using Recipes.Application.UseCases.Recipes.Queries.SearchRecipe;
using Recipes.WebApi.Dto.RecipeDtos;
using System.ComponentModel.DataAnnotations;

namespace Recipes.WebApi.Controllers
{
    [ApiController]
    [Route( "api/recipes" )]
    public class RecipesController( IMapper mapper ) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<int>> CreateRecipe( [FromBody] RecipeCreateDto dto,
            [FromServices] ICommandHandlerWithResult<CreateRecipeCommand, int> createRecipeCommandHandler )
        {
            CreateRecipeCommand command = mapper.Map<CreateRecipeCommand>( dto );
            Result<int> result = await createRecipeCommandHandler.HandleAsync( command );

            if ( !result.IsSuccess )
            {
                return BadRequest( result.Error );
            }

            return CreatedAtAction( nameof( GetRecipeById ), new { id = result.Value }, result.Value );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteRecipe( [FromRoute, Range( 1, int.MaxValue )] int id,
            [FromServices] ICommandHandler<DeleteRecipeCommand> deleteRecipeCommandHandler )
        {
            DeleteRecipeCommand command = new() { RecipeId = id };
            Result result = await deleteRecipeCommandHandler.HandleAsync( command );

            if ( !result.IsSuccess )
            {
                return BadRequest( result.Error );
            }

            return NoContent();
        }

        [HttpPut( "{id}" )]
        public async Task<IActionResult> UpdateRecipe( [FromRoute, Range( 1, int.MaxValue )] int id,
            [FromBody] RecipeUpdateDto dto, [FromServices] ICommandHandler<UpdateRecipeCommand> updateRecipeCommandHandler )
        {
            UpdateRecipeCommand command = mapper.Map<UpdateRecipeCommand>( dto );
            command.Id = id;

            Result result = await updateRecipeCommandHandler.HandleAsync( command );

            if ( !result.IsSuccess )
            {
                return BadRequest( result.Error );
            }

            return NoContent();
        }

        [HttpGet( "{id}" )]
        public async Task<ActionResult<RecipeReadDto>> GetRecipeById( [FromRoute, Range( 1, int.MaxValue )] int id,
            [FromServices] IQueryHandler<GetRecipeByIdQueryDto, GetRecipeByIdQuery> getRecipeByIdQueryHandler )
        {
            GetRecipeByIdQuery query = new() { Id = id };
            Result<GetRecipeByIdQueryDto> result = await getRecipeByIdQueryHandler.HandleAsync( query );

            if ( !result.IsSuccess )
            {
                return NotFound( result.Error );
            }

            return Ok( result.Value );
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GetRecipePartDto>>> SearchRecipes(
             [FromQuery] List<string> searchTerms,
             [FromServices] IQueryHandler<IEnumerable<GetRecipePartDto>, SearchRecipesQuery> searchRecipesQueryHandler,
             [FromQuery] int pageNumber = 1 )
        {
            SearchRecipesQuery query = new SearchRecipesQuery
            {
                SearchTerms = searchTerms,
                PageNumber = pageNumber
            };

            Result<IEnumerable<GetRecipePartDto>> result = await searchRecipesQueryHandler.HandleAsync( query );

            if ( !result.IsSuccess )
            {
                return BadRequest( result.Error );
            }

            return Ok( result.Value );
        }
    }
}

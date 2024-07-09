using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Recipes.Commands.CreateRecipe;
using Recipes.Application.Recipes.Commands.DeleteRecipe;
using Recipes.Application.Recipes.Commands.UpdateRecipe;
using Recipes.Application.Recipes.Dtos;
using Recipes.Application.Recipes.Queries.GetRecipeById;
using Recipes.Application.Recipes.Queries.GetAllRecipes;
using Application.CQRSInterfaces;

namespace Recipes.API.Controllers
{
    [ApiController]
    [Route( "api/recipes" )]
    public class RecipesController : ControllerBase
    {
        private readonly ICommandHandler<CreateRecipeCommand> _createRecipeCommandHandler;
        private readonly ICommandHandler<DeleteRecipeCommand> _deleteRecipeCommandHandler;
        private readonly ICommandHandler<UpdateRecipeCommand> _updateRecipeCommandHandler;
        private readonly IQueryHandler<GetRecipeByIdQueryDto, GetRecipeByIdQuery> _getRecipeByIdQueryHandler;
        private readonly IQueryHandler<IEnumerable<RecipeDto>, GetAllRecipesQuery> _getAllRecipesQueryHandler;

        public RecipesController(
            ICommandHandler<CreateRecipeCommand> createRecipeCommandHandler,
            ICommandHandler<DeleteRecipeCommand> deleteRecipeCommandHandler,
            ICommandHandler<UpdateRecipeCommand> updateRecipeCommandHandler,
            IQueryHandler<GetRecipeByIdQueryDto, GetRecipeByIdQuery> getRecipeByIdQueryHandler,
            IQueryHandler<IEnumerable<RecipeDto>, GetAllRecipesQuery> getAllRecipesQueryHandler )
        {
            _createRecipeCommandHandler = createRecipeCommandHandler;
            _deleteRecipeCommandHandler = deleteRecipeCommandHandler;
            _updateRecipeCommandHandler = updateRecipeCommandHandler;
            _getRecipeByIdQueryHandler = getRecipeByIdQueryHandler;
            _getAllRecipesQueryHandler = getAllRecipesQueryHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe( [FromBody] CreateRecipeCommand command )
        {
            var result = await _createRecipeCommandHandler.HandleAsync( command );
            if ( result.ValidationResult.IsFail )
            {
                return BadRequest( result.ValidationResult.Error );
            }
            return NoContent();
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteRecipe( int id )
        {
            var command = new DeleteRecipeCommand { RecipeId = id };
            var result = await _deleteRecipeCommandHandler.HandleAsync( command );
            if ( result.ValidationResult.IsFail )
            {
                return BadRequest( result.ValidationResult.Error );
            }
            return NoContent();
        }

        [HttpPut( "{id}" )]
        public async Task<IActionResult> UpdateRecipe( int id, [FromBody] UpdateRecipeCommand command )
        {
            if ( id != command.Id )
            {
                return BadRequest( "ID в URL не совпадает с ID в теле запроса" );
            }

            var result = await _updateRecipeCommandHandler.HandleAsync( command );
            if ( result.ValidationResult.IsFail )
            {
                return BadRequest( result.ValidationResult.Error );
            }
            return NoContent();
        }

        [HttpGet( "{id}" )]
        public async Task<IActionResult> GetRecipeById( int id )
        {
            var result = await _getRecipeByIdQueryHandler.HandleAsync( new GetRecipeByIdQuery { Id = id } );
            if ( result.ValidationResult.IsFail )
            {
                return NotFound( result.ValidationResult.Error );
            }
            return Ok( result.ObjResult );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipes()
        {
            var result = await _getAllRecipesQueryHandler.HandleAsync( new GetAllRecipesQuery() );
            if ( result.ValidationResult.IsFail )
            {
                return BadRequest( result.ValidationResult.Error );
            }
            return Ok( result.ObjResult );
        }
    }
}

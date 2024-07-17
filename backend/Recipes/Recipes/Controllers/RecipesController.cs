using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Recipes.Commands.DeleteRecipe;
using Recipes.Application.Recipes.Commands.UpdateRecipe;
using Recipes.Application.Recipes.Dtos;
using Recipes.Application.Recipes.Queries.GetAllRecipes;
using Application.CQRSInterfaces;
using Recipes.Application.Recipes.Commands;
using Recipes.Application.Recipes.Queries;
using Recipes.API.Dto.RecipeDtos;
using Newtonsoft.Json;
using Recipes.Domain.Entities;

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
        public async Task<IActionResult> CreateRecipe( [FromForm] IFormFile image, [FromForm] string recipeJson )
        {
            var dto = JsonConvert.DeserializeObject<RecipeCreateDto>( recipeJson );

            string fileName = null;

            if ( image != null )
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var folderPath = Path.Combine( currentDirectory, "../Recipes.Infrastructure/store" );
                fileName = Guid.NewGuid() + Path.GetExtension( image.FileName );
                var filePath = Path.Combine( folderPath, fileName );

                if ( !Directory.Exists( folderPath ) )
                {
                    Directory.CreateDirectory( folderPath );
                }

                if ( image.Length > 0 )
                {
                    using ( var stream = new FileStream( filePath, FileMode.Create ) )
                    {
                        await image.CopyToAsync( stream );
                    }
                }
            }

            var command = new CreateRecipeCommand
            {
                Name = dto.Name,
                Description = dto.Description,
                CookTime = dto.CookTime,
                CountPortion = dto.CountPortion,
                ImageUrl = fileName,
                Tags = dto.Tags,
                Ingredients = dto.Ingredients,
                Steps = dto.Steps
            };

            var result = await _createRecipeCommandHandler.HandleAsync( command );
            if ( result.ValidationResult.IsFail )
            {
                return BadRequest( result.ValidationResult.Error );
            }
            return NoContent();
        }



        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteRecipe( [FromRoute] int id )
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
        public async Task<IActionResult> UpdateRecipe( [FromRoute] int id, [FromBody] UpdateRecipeCommand command )
        {
            var updateCommand = new UpdateRecipeCommand
            {
                Id = id,
                Name = command.Name,
                Description = command.Description,
                CookTime = command.CookTime,
                CountPortion = command.CountPortion,
                ImageUrl = command.ImageUrl,
                Ingredients = command.Ingredients,
                Steps = command.Steps,
                Tags = command.Tags
            };

            var result = await _updateRecipeCommandHandler.HandleAsync( updateCommand );
            if ( result.ValidationResult.IsFail )
            {
                return BadRequest( result.ValidationResult.Error );
            }
            return NoContent();
        }

        [HttpGet( "{id}" )]
        public async Task<IActionResult> GetRecipeById( [FromRoute] int id )
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

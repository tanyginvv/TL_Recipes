using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Recipes.Commands.CreateRecipe;
using Recipes.Application.UseCases.Recipes.Commands.DeleteRecipe;
using Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Recipes.Queries.GetAllRecipes;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipeById;
using Recipes.Domain.Entities;
using Recipes.WebApi.Dto.RecipeDtos;


namespace Recipes.WebApi.Controllers
{
    [ApiController]
    [Route( "api/recipes" )]
    public class RecipesController( IMapper mapper ) : ControllerBase
    {
        private IMapper _mapper => mapper;

        [HttpPost]
        public async Task<IActionResult> CreateRecipe( [FromBody] RecipeCreateDto dto,
            [FromServices] ICommandHandler<CreateRecipeCommand> createRecipeCommandHandler )
        {
            var command = new CreateRecipeCommand
            {
                Name = dto.Name,
                Description = dto.Description,
                CookTime = dto.CookTime,
                CountPortion = dto.CountPortion,
                Tags = dto.Tags,
                ImageUrl = dto.ImageUrl,
                Ingredients = dto.Ingredients,
                Steps = dto.Steps
            };
            var result = await createRecipeCommandHandler.HandleAsync( command );

            if ( !result.IsSuccess )
            {
                return BadRequest( result.Error );
            }

            return Ok();
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteRecipe( [FromRoute, Range( 1, int.MaxValue )] int id,
            [FromServices] ICommandHandler<DeleteRecipeCommand> deleteRecipeCommandHandler
            )
        {
            var command = new DeleteRecipeCommand { RecipeId = id };
            var result = await deleteRecipeCommandHandler.HandleAsync( command );

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
            var command = new UpdateRecipeCommand
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                CookTime = dto.CookTime,
                CountPortion = dto.CountPortion,
                ImageUrl = dto.ImageUrl,
                Tags = dto.Tags,
                Ingredients = dto.Ingredients,
                Steps = dto.Steps
            };

            var result = await updateRecipeCommandHandler.HandleAsync( command );

            if ( !result.IsSuccess )
            {
                return BadRequest( result.Error );
            }

            return NoContent();
        }

        [HttpGet( "{id}" )]
        public async Task<IActionResult> GetRecipeById( [FromRoute, Range( 1, int.MaxValue )] int id,
            [FromServices] IQueryHandler<GetRecipeByIdQueryDto, GetRecipeByIdQuery> getRecipeByIdQueryHandler )
        {
            var result = await getRecipeByIdQueryHandler.HandleAsync( new GetRecipeByIdQuery { Id = id } );

            if ( !result.IsSuccess )
            {
                return NotFound( result.Error );
            }

            return Ok( result.Value );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipes( [FromServices] IQueryHandler<IEnumerable<RecipeDto>, GetAllRecipesQuery> getAllRecipesQueryHandler )
        {
            var result = await getAllRecipesQueryHandler.HandleAsync( new GetAllRecipesQuery() );

            if ( !result.IsSuccess )
            {
                return BadRequest( result.Error );
            }

            return Ok( result.Value );
        }
    }
}

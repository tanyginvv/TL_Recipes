using Infrastructure.JwtAuthorizations;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Commands.CreateRecipe;
using Recipes.Application.UseCases.Recipes.Commands.DeleteRecipe;
using Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipeById;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipes;
using Recipes.WebApi.Dto.RecipeDtos;
using System.ComponentModel.DataAnnotations;

namespace Recipes.WebApi.Controllers;

[ApiController]
[Route( "api/recipes" )]
public class RecipesController : ControllerBase
{
    [JwtAuthorization]
    [HttpPost]
    public async Task<ActionResult<RecipeReadIdDto>> CreateRecipe(
        [FromBody] RecipeCreateDto dto,
        [FromServices] ICommandHandlerWithResult<CreateRecipeCommand, RecipeIdDto> createRecipeCommandHandler,
        [FromServices] IImageTools imageTool )
    {
        CreateRecipeCommand command = dto.Adapt<CreateRecipeCommand>();
        Result<RecipeIdDto> result = await createRecipeCommandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            imageTool.DeleteImage( dto.ImageUrl );
            return BadRequest( result.Error );
        }

        return CreatedAtAction( nameof( GetRecipeById ), new { id = result.Value.Id }, result.Value );
    }

    [JwtAuthorization]
    [HttpDelete( "{id}" )]
    public async Task<IActionResult> DeleteRecipe(
        [FromRoute, Range( 1, int.MaxValue )] int id,
        [FromQuery] int userId,
        [FromServices] ICommandHandler<DeleteRecipeCommand> deleteRecipeCommandHandler )
    {
        DeleteRecipeCommand command = new DeleteRecipeCommand
        {
            RecipeId = id,
            UserId = userId
        };
        Result result = await deleteRecipeCommandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return NoContent();
    }

    [JwtAuthorization]
    [HttpPut( "{id}" )]
    public async Task<IActionResult> UpdateRecipe(
        [FromRoute, Range( 1, int.MaxValue )] int id,
        [FromBody] RecipeUpdateDto dto,
        [FromServices] ICommandHandler<UpdateRecipeCommand> updateRecipeCommandHandler,
        [FromServices] IImageTools imageTool )
    {
        UpdateRecipeCommand command = dto.Adapt<UpdateRecipeCommand>();
        command.Id = id;

        Result result = await updateRecipeCommandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            imageTool.DeleteImage( dto.ImageUrl );
            return BadRequest( result.Error );
        }

        return NoContent();
    }

    [HttpGet( "{id}" )]
    public async Task<ActionResult<RecipeReadDto>> GetRecipeById(
        [FromRoute, Range( 1, int.MaxValue )] int id,
        [FromServices] IQueryHandler<GetRecipeQueryDto, GetRecipeByIdQuery> getRecipeByIdQueryHandler )
    {
        GetRecipeByIdQuery query = new GetRecipeByIdQuery { Id = id };
        Result<GetRecipeQueryDto> result = await getRecipeByIdQueryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        return Ok( result.Value );
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecipePartReadDto>>> GetRecipes(
        [FromServices] IQueryHandler<IEnumerable<GetRecipePartDto>, GetRecipesQuery> getRecipesQueryHandler,
        [FromRoute] int userId = 0,
        [FromQuery] int pageNumber = 1,
        [FromQuery] List<string> searchTerms = null )
    {
        GetRecipesQuery query = new GetRecipesQuery
        {
            SearchTerms = searchTerms,
            PageNumber = pageNumber,
            UserId = userId
        };

        Result<IEnumerable<GetRecipePartDto>> result = await getRecipesQueryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok( result.Value );
    }
}
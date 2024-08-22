using Recipes.Infrastructure.JwtAuthorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Commands.CreateRecipe;
using Recipes.Application.UseCases.Recipes.Commands.DeleteRecipe;
using Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipeById;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipes;
using Recipes.WebApi.Dto.RecipeDtos;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Recipes.Application.Tokens.DecodeToken;

namespace Recipes.WebApi.Controllers;

[ApiController]
[Route( "api/recipes" )]
public class RecipesController( ITokenDecoder tokenDecoder ) : ControllerBase
{
    private int? GetUserIdFromAccessToken()
    {
        string accessToken = Request.Headers[ "Access-Token" ];
        if ( string.IsNullOrEmpty( accessToken ) )
        {
            return null;
        }

        JwtSecurityToken token = tokenDecoder.DecodeToken( accessToken );
        Claim userIdClaim = token.Claims.FirstOrDefault( claim => claim.Type == "userId" );

        if ( userIdClaim != null && int.TryParse( userIdClaim.Value, out int userId ) )
        {
            return userId;
        }

        return null;
    }

    [JwtAuthorization]
    [HttpPost]
    public async Task<ActionResult<RecipeReadIdDto>> CreateRecipe(
        [FromBody] RecipeCreateDto dto,
        [FromServices] ICommandHandlerWithResult<CreateRecipeCommand, RecipeIdDto> createRecipeCommandHandler )
    {
        int? userId = GetUserIdFromAccessToken();
        if ( userId is null )
        {
            return Unauthorized();
        }
        CreateRecipeCommand command = new()
        {
            AuthorId = userId.Value,
            Name = dto.Name,
            Description = dto.Description,
            CookTime = dto.CookTime,   
            PortionCount = dto.PortionCount,
            ImageUrl = dto.ImageUrl,
            Ingredients = ( ICollection<IngredientDto> )dto.Ingredients,
            Steps = ( ICollection<StepDto> )dto.Steps,
            Tags = ( ICollection<TagDto> )dto.Tags
        };

        Result<RecipeIdDto> result = await createRecipeCommandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return CreatedAtAction( nameof( GetRecipeById ), new { id = result.Value.Id }, result.Value );
    }

    [JwtAuthorization]
    [HttpDelete( "{id}" )]
    public async Task<ActionResult<Result>> DeleteRecipe(
        [FromRoute, Range( 1, int.MaxValue )] int id,
        [FromServices] ICommandHandler<DeleteRecipeCommand> deleteRecipeCommandHandler )
    {
        int? userId = GetUserIdFromAccessToken();
        if ( userId is null )
        {
            return Unauthorized();
        }

        DeleteRecipeCommand command = new DeleteRecipeCommand
        {
            RecipeId = id,
            AuthorId = userId.Value
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
    public async Task<ActionResult<Result>> UpdateRecipe(
        [FromRoute, Range( 1, int.MaxValue )] int id,
        [FromBody] RecipeUpdateDto dto,
        [FromServices] ICommandHandler<UpdateRecipeCommand> updateRecipeCommandHandler )
    {
        int? userId = GetUserIdFromAccessToken();
        if ( userId is null )
        {
            return Unauthorized();
        }

        UpdateRecipeCommand command = new() {
            Id = id,
            AuthorId = userId.Value,
            Name = dto.Name,
            Description = dto.Description,
            CookTime = dto.CookTime,
            PortionCount = dto.PortionCount,
            ImageUrl = dto.ImageUrl,
            Ingredients = ( ICollection<IngredientDto> )dto.Ingredients,
            Steps = ( ICollection<StepDto> )dto.Steps,
            Tags = ( ICollection<TagDto> )dto.Tags
        }; 

        Result result = await updateRecipeCommandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
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
        [FromQuery] int pageNumber = 1,
        [FromQuery] List<string> searchTerms = null )
    {
        int userId = GetUserIdFromAccessToken() ?? 0;

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
﻿using Recipes.WebApi.JwtAuthorization;
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
using Mapster;
using Recipes.WebApi.Extensions;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipeOfDay;

namespace Recipes.WebApi.Controllers;

[ApiController]
[Route( "api/recipes" )]
public class RecipesController : ControllerBase
{
    [JwtAuthorization]
    [HttpPost]
    public async Task<ActionResult<RecipeReadIdDto>> CreateRecipe(
        [FromBody] RecipeCreateDto dto,
        [FromServices] ICommandHandlerWithResult<CreateRecipeCommand, RecipeIdDto> createRecipeCommandHandler )
    {
        int userId = HttpContext.GetUserIdFromAccessToken();

        CreateRecipeCommand command = dto.Adapt<CreateRecipeCommand>();
        command.AuthorId = userId;

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
        int userId = HttpContext.GetUserIdFromAccessToken();

        DeleteRecipeCommand command = new DeleteRecipeCommand
        {
            RecipeId = id,
            AuthorId = userId
        };

        Result result = await deleteRecipeCommandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok();
    }

    [JwtAuthorization]
    [HttpPut()]
    public async Task<ActionResult<Result>> UpdateRecipe(
        [FromBody] RecipeUpdateDto dto,
        [FromServices] ICommandHandler<UpdateRecipeCommand> updateRecipeCommandHandler )
    {
        int userId = HttpContext.GetUserIdFromAccessToken();

        UpdateRecipeCommand command = dto.Adapt<UpdateRecipeCommand>();
        command.AuthorId = userId;

        Result result = await updateRecipeCommandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok();
    }

    [HttpGet( "{id}" )]
    public async Task<ActionResult<RecipeReadDto>> GetRecipeById(
        [FromRoute, Range( 1, int.MaxValue )] int id,
        [FromServices] IQueryHandler<GetRecipeQueryDto, GetRecipeByIdQuery> getRecipeByIdQueryHandler )
    {
        int userId = HttpContext.GetUserIdFromAccessToken();

        GetRecipeByIdQuery query = new GetRecipeByIdQuery { Id = id, UserId = userId };
        Result<GetRecipeQueryDto> result = await getRecipeByIdQueryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        return Ok( result.Value );
    }

    [HttpGet]
    public async Task<ActionResult<RecipeListReadDto>> GetRecipes(
        [FromServices] IQueryHandler<GetRecipesListDto, GetRecipesQuery> getRecipesQueryHandler,
        [FromQuery] int pageNumber = 1,
        [FromQuery] List<string> searchTerms = null,
        [FromQuery] RecipeQueryType recipeQueryType = RecipeQueryType.All )
    {
        int userId = HttpContext.GetUserIdFromAccessToken();

        GetRecipesQuery query = new GetRecipesQuery
        {
            SearchTerms = searchTerms,
            PageNumber = pageNumber,
            UserId = userId,
            RecipeQueryType = recipeQueryType
        };

        Result<GetRecipesListDto> result = await getRecipesQueryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok( result.Value );
    }

    [HttpGet( "recipe-of-day" )]
    public async Task<ActionResult<RecipeReadDto>> GetRecipeOfDay(
        [FromServices] IQueryHandler<GetRecipeOfDayDto, GetRecipeOfDayQuery> getRecipeOfDayQueryHandler )
    {
        GetRecipeOfDayQuery query = new GetRecipeOfDayQuery { };
        Result<GetRecipeOfDayDto> result = await getRecipeOfDayQueryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok( result.Value );
    }
}
using Infrastructure.JwtAuthorizations;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipes;
using Recipes.Application.UseCases.Users.Commands.DeleteUser;
using Recipes.Application.UseCases.Users.Commands.UpdateUser;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.Application.UseCases.Users.Queries.GetUserById;
using Recipes.WebApi.Dto.UserDto;

namespace Recipes.WebApi.Controllers;

[ApiController]
[Route( "api/users" )]
public class UsersController() : ControllerBase
{

    [HttpGet( "login/{id}" )]
    public async Task<ActionResult<ReadUserDto>> GetUserLoginById( int id,
        [FromServices] IQueryHandler<GetUserLoginByIdQueryDto, GetUserLoginByIdQuery> getUserLoginByIdQueryHandler )
    {
        GetUserLoginByIdQuery query = new GetUserLoginByIdQuery { Id = id };
        Result<GetUserLoginByIdQueryDto> result = await getUserLoginByIdQueryHandler.HandleAsync( query );
        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        GetUserLoginByIdQueryDto user = result.Value;
        ReadUserDto userDto = new ReadUserDto
        {
            Id = user.Id,
            Login = user.Login
        };

        return Ok( userDto );
    }

    [HttpGet( "{id}" )]
    public async Task<ActionResult<UserDto>> GetUserById( int id,
         [FromServices] IQueryHandler<GetUserByIdQueryDto, GetUserByIdQuery> getUserByIdQueryHandler )
    {
        GetUserByIdQuery query = new GetUserByIdQuery { Id = id };
        Result<GetUserByIdQueryDto> result = await getUserByIdQueryHandler.HandleAsync( query );
        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        GetUserByIdQueryDto user = result.Value;
        UserDto userDto = new UserDto
        {
            Id = user.Id,
            Login = user.Login,
            Name = user.Name,
            Description = user.Description,
            RecipesCount = user.RecipesCount
        };

        return Ok( userDto );
    }

    [JwtAuthorization]
    [HttpPut( "{id}" )]
    public async Task<IActionResult> UpdateUser(
        int id,
        UpdateUserDto updateUserDto,
        [FromServices] ICommandHandler<UpdateUserCommand> updateUserCommandHandler )
    {
        UpdateUserCommand command = new UpdateUserCommand
        {
            Id = id,
            Name = updateUserDto.Name,
            Description = updateUserDto.Description,
            Login = updateUserDto.Login,
            OldPasswordHash = updateUserDto.OldPasswordHash,
            NewPasswordHash = updateUserDto.NewPasswordHash
        };

        Result result = await updateUserCommandHandler.HandleAsync( command );
        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return NoContent();
    }

    [JwtAuthorization]
    [HttpDelete( "{id}" )]
    public async Task<IActionResult> DeleteUser(
        int id,
        [FromQuery] string passwordHash,
        [FromServices] ICommandHandler<DeleteUserCommand> deleteUserCommandHandler )
    {
        DeleteUserCommand command = new DeleteUserCommand
        {
            UserId = id,
            PasswordHash = passwordHash
        };

        Result result = await deleteUserCommandHandler.HandleAsync( command );
        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return NoContent();
    }

    [JwtAuthorization]
    [HttpGet( "{userId}/recipes" )]
    public async Task<IActionResult> GetRecipes(
        [FromServices] IQueryHandler<IEnumerable<GetRecipePartDto>, GetRecipesQuery> getRecipesQueryHandler,
        [FromRoute] int userId = 0,
        [FromQuery] int pageNumber = 1,
        [FromQuery] List<string> searchTerms = null )
    {
        GetRecipesQuery query = new GetRecipesQuery
        {
            UserId = userId,
            PageNumber = pageNumber,
            SearchTerms = searchTerms
        };

        Result<IEnumerable<GetRecipePartDto>> result = await getRecipesQueryHandler.HandleAsync( query );
        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok( result.Value );
    }
}
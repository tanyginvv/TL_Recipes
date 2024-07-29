using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Recipes.Queries.SearchRecipe;
using Recipes.Application.UseCases.Users.Commands.DeleteUser;
using Recipes.Application.UseCases.Users.Commands.UpdateUser;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.Application.UseCases.Users.Queries.GetUserById;
using Recipes.WebApi.Dto.UserDto;

namespace Recipes.WebApi.Controllers
{
    [ApiController]
    [Route( "api/[controller]" )]
    public class UsersController
        (
            ICommandHandler<DeleteUserCommand> deleteUserCommandHandler,
            ICommandHandler<UpdateUserCommand> updateUserCommandHandler,
            IQueryHandler<GetUserByIdQueryDto, GetUserByIdQuery> getUserByIdQueryHandler,
            IQueryHandler<IEnumerable<GetRecipePartDto>, GetRecipesQuery> getRecipesQueryHandler ) : ControllerBase
    {

        [HttpGet( "{id}" )]
        public async Task<ActionResult<UserDto>> GetUserById( int id )
        {
            var query = new GetUserByIdQuery { Id = id };
            var result = await getUserByIdQueryHandler.HandleAsync( query );
            if ( !result.IsSuccess )
            {
                return NotFound( result.Error );
            }

            var user = result.Value;
            var userDto = new ReadUserDto
            {
                Id = user.Id,
                Login = user.Login
            };

            return Ok( userDto );
        }

        [Authorize]
        [HttpPut( "{id}" )]
        public async Task<IActionResult> UpdateUser( int id, UpdateUserDto updateUserDto )
        {
            var command = new UpdateUserCommand
            {
                Id = id,
                Name = updateUserDto.Name,
                Description = updateUserDto.Description,
                Login = updateUserDto.Login,
                OldPasswordHash = updateUserDto.OldPasswordHash,
                NewPasswordHash = updateUserDto.NewPasswordHash
            };

            var result = await updateUserCommandHandler.HandleAsync( command );
            if ( !result.IsSuccess )
            {
                return BadRequest( result.Error );
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteUser( int id, [FromQuery] string passwordHash )
        {
            var command = new DeleteUserCommand
            {
                userId = id,
                PasswordHash = passwordHash
            };

            var result = await deleteUserCommandHandler.HandleAsync( command );
            if ( !result.IsSuccess )
            {
                return BadRequest( result.Error );
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet( "{userId}/Recipes" )]
        public async Task<IActionResult> GetRecipes(
            [FromRoute] int userId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] List<string> searchTerms = null )
        {
            var query = new GetRecipesQuery
            {
                UserId = userId,
                PageNumber = pageNumber,
                SearchTerms = searchTerms
            };

            var result = await getRecipesQueryHandler.HandleAsync( query );
            if ( !result.IsSuccess )
            {
                return BadRequest( result.Error );
            }

            return Ok( result.Value );
        }
    }
}

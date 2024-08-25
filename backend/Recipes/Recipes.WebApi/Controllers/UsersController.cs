using Recipes.WebApi.JwtAuthorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Commands.UpdateUser;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.Application.UseCases.Users.Queries.GetUserById;
using Recipes.Application.UseCases.Users.Queries.GetUserNameById;
using Recipes.WebApi.Dto.UserDtos;
using Recipes.Application.UseCases.Users.Commands.CreateUser;
using Recipes.Application.Interfaces;
using Recipes.Application.UseCases.Services;
using Mapster;
using Recipes.WebApi.Extensions;

namespace Recipes.WebApi.Controllers;

[ApiController]
[Route( "api/users" )]
public class UsersController : ControllerBase
{
    //[JwtAuthorization]
    [HttpGet( "name" )]
    public async Task<ActionResult<ReadUserDto>> GetUserNameById(
        [FromServices] IQueryHandler<GetUserNameByIdQueryDto, GetUserNameByIdQuery> getUserNameByIdQueryHandler )
    {
        int userId = 2; /*HttpContext.GetUserIdFromAccessToken();*/

        GetUserNameByIdQuery query = new GetUserNameByIdQuery { Id = userId };

        Result<GetUserNameByIdQueryDto> result = await getUserNameByIdQueryHandler.HandleAsync( query );
        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        GetUserNameByIdQueryDto user = result.Value;
        ReadUserDto userDto = new ReadUserDto
        {
            Name = user.Name
        };

        return Ok( userDto );
    }

    //[JwtAuthorization]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetUser( 
        [FromServices] IQueryHandler<GetUserByIdQueryDto, GetUserByIdQuery> getUserByIdQueryHandler )
    {
        int userId = 2;/*HttpContext.GetUserIdFromAccessToken();*/

        GetUserByIdQuery query = new GetUserByIdQuery { Id = userId };
        Result<GetUserByIdQueryDto> result = await getUserByIdQueryHandler.HandleAsync( query );
        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        GetUserByIdQueryDto user = result.Value;
        UserDto userDto = user.Adapt<UserDto>();

        return Ok( userDto );
    }

    //[JwtAuthorization]
    [HttpPut()]
    public async Task<ActionResult<Result>> UpdateUser(
        [FromBody] UpdateUserDto updateUserDto,
        [FromServices] ICommandHandler<UpdateUserCommand> updateUserCommandHandler )
    {
        int userId = 2;/*HttpContext.GetUserIdFromAccessToken();*/

        UpdateUserCommand command = updateUserDto.Adapt<UpdateUserCommand>();
        command.Id = userId;

        Result result = await updateUserCommandHandler.HandleAsync( command );
        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok();
    }

    [HttpPost]
    [Route( "register" )]
    public async Task<ActionResult<Result>> Register( 
        [FromBody] RegisterUserDto registerUserDto,
        [FromServices] ICommandHandler<CreateUserCommand> createUserCommandHandler )
    {
        CreateUserCommand createUserCommand = registerUserDto.Adapt<CreateUserCommand>();
        Result commandResult = await createUserCommandHandler.HandleAsync( createUserCommand );

        if ( !commandResult.IsSuccess )
        {
            return BadRequest( commandResult.Error );
        }

        return Ok( commandResult );
    }

    [HttpPost( "refresh-token" )]
    public async Task<ActionResult<TokenDto>> RefreshToken(
         [FromServices] IRefreshTokenService refreshTokenService )
    {
        string refreshTokenFromCookie = Request.Cookies[ "RefreshToken" ];

        Result<TokenDto> commandResult = await refreshTokenService.RefreshTokenAsync( refreshTokenFromCookie );

        if ( !commandResult.IsSuccess )
        {
            return BadRequest( commandResult.Error );
        }

        return Ok( commandResult.Value );
    }

    [HttpPost( "login" )]
    public async Task<ActionResult<TokenDto>> Login( 
        [FromBody] LoginDto loginDto,
        [FromServices] ILoginService service )
    {
        Result<TokenDto> commandResult = await service.LoginAsync( loginDto.Login, loginDto.Password );

        if ( !commandResult.IsSuccess )
        {
            return BadRequest( commandResult.Error );
        }

        return Ok( commandResult.Value );
    }
}
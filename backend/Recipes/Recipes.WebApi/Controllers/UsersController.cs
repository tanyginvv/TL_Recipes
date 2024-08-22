using Recipes.Infrastructure.JwtAuthorization;
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
using Recipes.Application.Tokens.DecodeToken;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Recipes.Application.UseCases.Services;

namespace Recipes.WebApi.Controllers;

[ApiController]
[Route( "api/users" )]
public class UsersController( ITokenDecoder tokenDecoder ) : ControllerBase
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
    [HttpGet( "name" )]
    public async Task<ActionResult<ReadUserDto>> GetUserNameById(
        [FromServices] IQueryHandler<GetUserNameByIdQueryDto, GetUserNameByIdQuery> getUserNameByIdQueryHandler )
    {
        int? userId = GetUserIdFromAccessToken();
        if ( userId is null )
        {
            return Unauthorized();
        }
        GetUserNameByIdQuery query = new GetUserNameByIdQuery { Id = userId.Value };

        Result<GetUserNameByIdQueryDto> result = await getUserNameByIdQueryHandler.HandleAsync( query );
        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        GetUserNameByIdQueryDto user = result.Value;
        ReadUserDto userDto = new ReadUserDto
        {
            Id = user.Id,
            Name = user.Name
        };

        return Ok( userDto );
    }


    [JwtAuthorization]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetUser( 
        [FromServices] IQueryHandler<GetUserByIdQueryDto, GetUserByIdQuery> getUserByIdQueryHandler )
    {
        int? userId = GetUserIdFromAccessToken();
        if ( userId is null )
        {
            return Unauthorized();
        }

        GetUserByIdQuery query = new GetUserByIdQuery { Id = userId.Value };
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
    [HttpPut()]
    public async Task<ActionResult<Result>> UpdateUser(
        [FromBody] UpdateUserDto updateUserDto,
        [FromServices] ICommandHandler<UpdateUserCommand> updateUserCommandHandler )
    {
        int? userId = GetUserIdFromAccessToken();
        if ( userId is null )
        {
            return Unauthorized();
        }

        UpdateUserCommand command = new UpdateUserCommand
        {
            Id = userId.Value,
            Name = updateUserDto.Name,
            Description = updateUserDto.Description,
            Login = updateUserDto.Login,
            OldPassword = updateUserDto.OldPassword,
            NewPassword = updateUserDto.NewPassword
        };

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
        CreateUserCommand createUserCommand = new CreateUserCommand
        {
            Login = registerUserDto.Login,
            Password = registerUserDto.Password,
            Name = registerUserDto.Name,
        };
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
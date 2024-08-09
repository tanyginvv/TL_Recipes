using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.UseCases.Users.Commands.AuthenticatePassword;
using Recipes.Application.UseCases.Users.Commands;
using Recipes.Application.UseCases.Users.Dto;
using Recipes.WebApi.Dto.UserDto;
using Recipes.Application.Results;
using Recipes.Application.UseCases.UserAuthorizationTokens.Dto;
using Recipes.Application.UseCases.UserAuthorizationTokens.RefreshToken;
using Recipes.WebApi.Dto.AuthenticationDto;

namespace Presentation.Intranet.Api.Controllers;

[ApiController]
[Route( "api/users" )]
public class AuthenticationController() : ControllerBase
{
    [HttpPost]
    [Route( "registrate" )]
    public async Task<IActionResult> Registrate( [FromBody] RegistrateUserDto registrateUserDto,
        [FromServices] ICommandHandler<CreateUserCommand> createUserCommandHandler )
    {
        CreateUserCommand createUserCommand = new CreateUserCommand
        {
            Login = registrateUserDto.Login,
            PasswordHash = registrateUserDto.PasswordHash,
            Name = registrateUserDto.Name,
            Description = registrateUserDto.Description
        };
        Result commandResult = await createUserCommandHandler.HandleAsync( createUserCommand );

        if ( !commandResult.IsSuccess )
        {
            return BadRequest( commandResult.Error );
        }

        return Ok( commandResult );
    }

    [HttpPost( "refresh-token" )]
    public async Task<IActionResult> RefreshToken(
        [FromServices] ICommandHandlerWithResult<RefreshTokenCommand, RefreshTokenCommandDto> refreshTokenCommandHandler )
    {
        string refreshTokenFromCookie = Request.Cookies[ "RefreshToken" ];

        RefreshTokenCommand refreshTokenCommand = new RefreshTokenCommand
        {
            RefreshToken = refreshTokenFromCookie,
        };
        Result<RefreshTokenCommandDto> commandResult = await refreshTokenCommandHandler.HandleAsync( refreshTokenCommand );

        if ( !commandResult.IsSuccess )
        {
            return BadRequest( commandResult.Value );
        }

        return Ok( commandResult.Value );
    }

    [HttpPost( "authentication" )]
    public async Task<IActionResult> Authentication( [FromBody] AuthenticationDto authenticationDto,
        [FromServices] ICommandHandlerWithResult<AuthenticateUserCommand, AuthenticateUserCommandDto> authenticateCommandHandler )
    {
        AuthenticateUserCommand authenticateUserCommand = new AuthenticateUserCommand
        {
            Login = authenticationDto.Login,
            PasswordHash = authenticationDto.PasswordHash
        };

        Result<AuthenticateUserCommandDto> commandResult = await authenticateCommandHandler
            .HandleAsync( authenticateUserCommand );

        if ( !commandResult.IsSuccess )
        {
            return BadRequest( commandResult.Error );
        }

        return Ok( commandResult.Value );
    }
}
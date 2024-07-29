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

namespace Presentation.Intranet.Api.Controllers
{
    [ApiController]
    [Route( "Api/Users" )]
    public class AuthenticationController( ICommandHandler<CreateUserCommand> createUserCommandHandler,
            ICommandHandlerWithResult<AuthenticateUserCommand, AuthenticateUserCommandDto> authenticateCommandHandler,
            ICommandHandlerWithResult<RefreshTokenCommand, RefreshTokenCommandDto> refreshTokenCommandHandler ) : ControllerBase
    {
        [HttpPost]
        [Route( "Registrate" )]
        public async Task<IActionResult> Registrate( [FromBody] RegistrateUserDto registrateUserDto )
        {
            CreateUserCommand createUserCommand = new CreateUserCommand
            {
                Login = registrateUserDto.Login,
                PasswordHash = registrateUserDto.PasswordHash,
                Name = registrateUserDto.Name
            };
            Result commandResult = await createUserCommandHandler.HandleAsync( createUserCommand );

            if ( !commandResult.IsSuccess )
            {
                return BadRequest( commandResult );
            }

            return Ok( commandResult );
        }

        [HttpPost( "Refresh-Token" )]
        public async Task<IActionResult> RefreshToken()
        {
            string refreshTokenFromCookie = Request.Cookies[ "RefreshToken" ];

            RefreshTokenCommand refreshTokenCommand = new RefreshTokenCommand
            {
                RefreshToken = refreshTokenFromCookie,
            };
            Result<RefreshTokenCommandDto> commandResult = await refreshTokenCommandHandler.HandleAsync( refreshTokenCommand );

            if ( !commandResult.IsSuccess )
            {
                return BadRequest( commandResult );
            }

            return Ok( commandResult );
        }

        [HttpPost( "Authentication" )]
        public async Task<IActionResult> Authentication( [FromBody] AuthenticationDto authenticationDto )
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
                return BadRequest( commandResult );
            }
            Response.Cookies.Append( "AccessToken", commandResult.Value.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes( 30 )
            } );

            Response.Cookies.Append( "RefreshToken", commandResult.Value.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays( 7 )
            } );

            return Ok( commandResult );
        }
    }
}
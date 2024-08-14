namespace Recipes.Application.Tokens.CreateToken;

public interface ITokenCreator
{
    string GenerateAccessToken( int userId );

    string GenerateRefreshToken();
}
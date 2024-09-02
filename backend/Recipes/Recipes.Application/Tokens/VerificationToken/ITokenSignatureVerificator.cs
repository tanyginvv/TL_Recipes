namespace Recipes.Application.Tokens.VerificationToken;

public interface ITokenSignatureVerificator
{
    bool VerifySignature( string accessToken, string secret );
}
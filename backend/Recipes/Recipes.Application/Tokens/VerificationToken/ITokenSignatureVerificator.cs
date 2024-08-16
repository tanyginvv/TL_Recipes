namespace Recipes.Application.Tokens.VerificationToken;

public interface ITokenSignatureVerificator
{
    void VerifySignature( string accessToken, string secret );
}
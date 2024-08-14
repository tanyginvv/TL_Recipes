namespace Recipes.Application.Tokens.VerificationToken;

public interface ITokenSignatureVerificator
{
    void VerifySignature();
    string Base64UrlEncode( byte[] input );
}
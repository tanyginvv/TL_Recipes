using System.Security.Cryptography;
using System.Text;
using Recipes.Application.Tokens.VerificationToken;

namespace Recipes.Infrastructure.Tokens.VerificationToken;

public class TokenSignatureVerificator : ITokenSignatureVerificator
{
    public bool VerifySignature( string accessToken, string secret )
    {
        string[] parts = accessToken.Split( ".".ToCharArray() );
        string header = parts[ 0 ];
        string payload = parts[ 1 ];
        string signature = parts[ 2 ];

        byte[] bytesToSign = Encoding.UTF8.GetBytes( string.Join( ".", header, payload ) );
        byte[] bytesToSecret = Encoding.UTF8.GetBytes( secret );

        HMACSHA256 alg = new HMACSHA256( bytesToSecret );
        byte[] hash = alg.ComputeHash( bytesToSign );

        string computedSignature = Base64UrlEncode( hash );

        return signature == computedSignature;
    }

    private static string Base64UrlEncode( byte[] input )
    {
        string output = Convert.ToBase64String( input );
        output = output.Split( '=' )[ 0 ];
        output = output.Replace( '+', '-' );
        output = output.Replace( '/', '_' );
        return output;
    }
}
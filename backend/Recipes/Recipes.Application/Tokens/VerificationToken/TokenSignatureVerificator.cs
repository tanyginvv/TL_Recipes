using System.Security.Cryptography;
using System.Text;

namespace Recipes.Application.Tokens.VerificationToken;

public class TokenSignatureVerificator( string accessToken, string secret )
{
    public bool TokenIsValid;

    public void VerifySignature()
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

        TokenIsValid = signature == computedSignature;
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
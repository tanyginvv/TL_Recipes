namespace Recipes.Application.Results
{
    public record ResultError( string Message );

    public class Result
    {
        public readonly ResultError Error;

        public bool IsSuccess => Error is null;

        public static Result Success { get; } = FromSuccess();

        private Result( ResultError error )
        {
            Error = error;
        }

        public static Result FromSuccess()
        {
            return new Result( null );
        }

        public static Result FromError( ResultError error )
        {
            return new Result( error );
        }

        public static Result FromError( string errorText )
        {
            return new Result( new ResultError( errorText ) );
        }
    }
}
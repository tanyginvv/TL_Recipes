namespace Recipes.Application.Results
{
    public record ResultError( string Message );

    public class Result
    {
        public readonly ResultError? Error;

        public bool IsSuccess => Error is null;

        public static Result Success { get; } = FromSuccess();

        private Result( ResultError? error )
        {
            Error = error;
        }

        public static Result FromSuccess()
        {
            return new Result( null );
        }

        public static Result FromError( ResultError? error )
        {
            return new Result( error );
        }

        public static Result FromError( string errorText )
        {
            return new Result( new ResultError( errorText ) );
        }
    }

    public class Result<T>
    {
        public readonly T? Value;

        public readonly ResultError? Error;

        public bool IsSuccess => Error is null;

        private Result( T value )
        {
            Value = value;
        }

        private Result( ResultError? error )
        {
            Error = error;
        }

        public static Result<T> FromSuccess( T value )
        {
            return new Result<T>( value );
        }

        public static Result<T> FromError( ResultError? error )
        {
            return new Result<T>( error );
        }

        public static Result<T> FromError( string errorText )
        {
            return new Result<T>( new ResultError( errorText ) );
        }

        public static Result<T> FromError( Result result )
        {
            return new Result<T>( result.Error );
        }
    }
}
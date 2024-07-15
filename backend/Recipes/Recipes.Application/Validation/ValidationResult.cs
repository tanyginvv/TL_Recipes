namespace Application.Validation
{
    public class ValidationResult
    {
        public bool IsFail => Error != null;
        public string Error { get; private set; }

        private ValidationResult( string error = null )
        {
            Error = error;
        }

        public static ValidationResult Ok()
        {
            return new ValidationResult();
        }
        public static ValidationResult Fail( string error )
        {
            return new ValidationResult( error );
        }
    }
}
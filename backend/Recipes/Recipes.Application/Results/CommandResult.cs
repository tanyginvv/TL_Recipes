using Recipes.Application.Validation;

namespace Recipes.Application.Results
{
    public class CommandResult
    {
        public ValidationResult ValidationResult { get; private set; }

        public CommandResult( ValidationResult validationResult )
        {
            ValidationResult = validationResult;
        }
    }

    public class CommandResult<TCommandResultData> where TCommandResultData : class
    {
        public ValidationResult ValidationResult { get; private set; }
        public TCommandResultData ObjResult { get; private set; }

        public CommandResult( TCommandResultData objResult )
        {
            ObjResult = objResult;
            ValidationResult = ValidationResult.Ok();
        }

        public CommandResult( ValidationResult validationResult )
        {
            ValidationResult = validationResult;
        }
    }
}
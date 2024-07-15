namespace Application.Validation
{
    public interface IAsyncValidator<TData> where TData : class
    {
        Task<ValidationResult> ValidationAsync( TData inputData );
    }
}
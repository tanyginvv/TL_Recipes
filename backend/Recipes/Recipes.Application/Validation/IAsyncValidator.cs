using Recipes.Application.Results;

namespace Recipes.Application.Validation
{
    public interface IAsyncValidator<in TData> where TData : class
    {
        Task<Result> ValidationAsync( TData inputData );
    }
}
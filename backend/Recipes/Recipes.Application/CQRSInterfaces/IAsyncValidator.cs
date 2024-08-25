using Recipes.Application.Results;

namespace Recipes.Application.CQRSInterfaces;

public interface IAsyncValidator<in TData> where TData : class
{
    Task<Result> ValidateAsync( TData inputData );
}
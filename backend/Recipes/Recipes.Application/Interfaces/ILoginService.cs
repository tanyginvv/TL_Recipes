using Recipes.Application.Results;
using Recipes.Application.UseCases.Services;

namespace Recipes.Application.Interfaces;

public interface ILoginService
{
    Task<Result<TokenDto>> LoginAsync( string login, string password );
}
﻿using Recipes.Domain.Entities;

namespace Recipes.Application.Repositories
{
    public interface IIngredientRepository
    {
        Task<IReadOnlyList<Ingredient>> GetByRecipeIdAsync( int recipeId );
        Task<Ingredient> GetByIdAsync( int id );
        Task AddIngredientAsync( Ingredient ingredient );
        Task UpdateIngredientAsync( Ingredient ingredient );
        Task DeleteByIdAsync( int id );
    }
}
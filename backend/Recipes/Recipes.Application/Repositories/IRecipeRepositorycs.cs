﻿using Recipes.Application.Tags.Dtos;
using Recipes.Domain.Entities;

namespace Application.Repositories
{
    public interface IRecipeRepository
    {
        Task AddAsync( Recipe recipe );
        Task DeleteAsync( int id );
        Task<IReadOnlyList<Recipe>> GetAllAsync();
        Task<Recipe> GetByIdAsync( int id );
        Task UpdateAsync( Recipe recipe );
    }
}
﻿using Recipes.Domain.Entities;

namespace Recipes.Application.Ingredients.Queries
{
    public class GetIngredientsByRecipeIdQuery
    {
        public int RecipeId { get; init; }
    }
}
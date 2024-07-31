﻿using Recipes.Application.UseCases.Recipes.Dtos;

namespace Recipes.Application.UseCases.Tags.Commands
{
    public class UpdateTagsCommand
    {
        public required int RecipeId { get; init; }
        public required IEnumerable<TagDto> RecipeTags { get; init; }
    }
}

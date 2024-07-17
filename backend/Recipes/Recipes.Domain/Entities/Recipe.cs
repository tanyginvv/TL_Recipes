﻿namespace Recipes.Domain.Entities
{
    public class Recipe
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CookTime { get; set; }
        public int CountPortion { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<Tag> Tags { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<Step> Steps { get; set; }

        public Recipe( string name, string description, int cookTime, int countPortion, string imageUrl )
        {
            Name = name;
            Description = description;
            CookTime = cookTime;
            CountPortion = countPortion;
            ImageUrl = imageUrl;
            Tags = new List<Tag>();
            Ingredients = new List<Ingredient>();
            Steps = new List<Step>();
        }
    }
}

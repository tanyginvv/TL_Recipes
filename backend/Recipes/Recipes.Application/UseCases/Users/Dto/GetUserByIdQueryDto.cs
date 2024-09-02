﻿namespace Recipes.Application.UseCases.Users.Dto;

public class GetUserByIdQueryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Login { get; set; }
    public int RecipeCount { get; set; }
}
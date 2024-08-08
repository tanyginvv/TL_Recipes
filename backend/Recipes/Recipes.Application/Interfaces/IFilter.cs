﻿namespace Recipes.Application.Interfaces;

public interface IFilter<T>
{
    IQueryable<T> Apply( IQueryable<T> query );
}
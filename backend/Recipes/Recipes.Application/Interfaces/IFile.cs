﻿namespace Recipes.Application.Interfaces;

public interface IFile
{
    string FileName { get; }
    long Length { get; }
    Stream OpenReadStream();
}
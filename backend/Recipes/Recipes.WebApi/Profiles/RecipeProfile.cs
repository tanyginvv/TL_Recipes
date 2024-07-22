using AutoMapper;
using Recipes.Application.UseCases.Recipes.Commands.CreateRecipe;
using Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.WebApi.Dto.RecipeDtos;

namespace Recipes.WebApi.Profiles
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<RecipeCreateDto, CreateRecipeCommand>();
            CreateMap<RecipeUpdateDto, UpdateRecipeCommand>()
                .ForMember( dest => dest.Id, opt => opt.Ignore() );
            CreateMap<RecipeDto, RecipeUpdateDto>();
            CreateMap<RecipeDto, RecipeCreateDto>();
        }
    }
}

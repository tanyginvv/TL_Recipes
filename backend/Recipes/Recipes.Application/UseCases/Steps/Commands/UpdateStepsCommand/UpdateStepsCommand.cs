using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Commands
{
    public class UpdateStepsCommand
    {
        public Recipe Recipe { get; set; }
        public IEnumerable<StepDto> NewSteps { get; set; }
    }
}

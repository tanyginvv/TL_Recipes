using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Steps.Commands
{
    public class UpdateStepsCommandValidator : IAsyncValidator<UpdateStepsCommand>
    {
        public async Task<Result> ValidateAsync( UpdateStepsCommand command )
        {
            if ( command.Recipe is null )
            {
                return Result.FromError( "Рецепт не может быть null." );
            }

            foreach ( StepDto step in command.NewSteps )
            {
                if ( step.StepNumber <= 0 )
                {
                    return Result.FromError( "Номер шага должен быть больше нуля." );
                }

                if ( string.IsNullOrEmpty( step.StepDescription ) )
                {
                    return Result.FromError( "Описание шага не может быть пустым." );
                }

                if ( step.StepDescription.Length > 250 )
                {
                    return Result.FromError( "Описание шага не может быть больше чем 250 символов." );
                }
            }

            return Result.Success;
        }
    }
}
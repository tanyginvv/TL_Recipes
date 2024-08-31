using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Commands.CreateStep;

public class CreateStepCommandHandler(
    IStepRepository stepRepository,
    IAsyncValidator<CreateStepCommand> validator )
    : CommandBaseHandlerWithResult<CreateStepCommand, Step>( validator )
{
    protected override async Task<Result<Step>> HandleAsyncImpl( CreateStepCommand command )
    {
        Step step = new Step( command.StepNumber, command.StepDescription, command.Recipe.Id );

        await stepRepository.AddAsync( step );

        return Result<Step>.FromSuccess( step );
    }
}
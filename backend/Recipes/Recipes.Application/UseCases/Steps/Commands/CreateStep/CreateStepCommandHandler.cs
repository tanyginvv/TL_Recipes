using Microsoft.Extensions.Logging;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Commands.CreateStep;

public class CreateStepCommandHandler(
    IStepRepository stepRepository,
    IAsyncValidator<CreateStepCommand> validator,
    ILogger<CreateStepCommand> logger )
    : CommandBaseHandlerWithResult<CreateStepCommand, Step>( validator, logger )
{
    protected override async Task<Result<Step>> HandleImplAsync( CreateStepCommand command )
    {
        Step step = new Step( command.StepNumber, command.StepDescription, command.Recipe.Id );

        await stepRepository.AddAsync( step );

        return Result<Step>.FromSuccess( step );
    }
}
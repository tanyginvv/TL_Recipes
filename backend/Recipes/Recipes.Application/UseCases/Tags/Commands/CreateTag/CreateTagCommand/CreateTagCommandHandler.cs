using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.Validation;
using Recipes.Application.Results;
using Recipes.Application.Repositories;

namespace Recipes.Application.UseCases.Tags.Commands.CreateTag.CreateTagCommand
{
    public class CreateTagCommandHandler(
            ITagRepository tagRepository,
            IAsyncValidator<CreateTagCommand> validator )
        : ICommandHandler<CreateTagCommand>
    {
        public async Task<Result> HandleAsync( CreateTagCommand createTagCommand )
        {
            Result validationResult = await validator.ValidateAsync( createTagCommand );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error );
            }

            Tag tag = new Tag( createTagCommand.Name );

            await tagRepository.AddAsync( tag );

            return Result.Success;
        }
    }
}

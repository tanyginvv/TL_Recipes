using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.Validation;
using Recipes.Application.Results;
using Recipes.Application.Repositories;

namespace Recipes.Application.UseCases.Tags.Commands.CreateTag
{
    public class CreateTagCommandHandler(
            ITagRepository tagRepository,
            IAsyncValidator<CreateTagCommand> validator )
        : ICommandHandlerWithResult<CreateTagCommand, Tag>
    {
        public async Task<Result<Tag>> HandleAsync( CreateTagCommand createTagCommand )
        {
            Result validationResult = await validator.ValidateAsync( createTagCommand );
            if ( !validationResult.IsSuccess )
            {
                return Result<Tag>.FromError( validationResult.Error );
            }

            bool tagExists = await tagRepository.ContainsAsync( tag => tag.Name == createTagCommand.Name );

            if ( tagExists )
            {
                Tag existingTag = await tagRepository.GetByNameAsync( createTagCommand.Name );
                return Result<Tag>.FromSuccess( existingTag );
            }

            Tag tag = new Tag( createTagCommand.Name );
            await tagRepository.AddAsync( tag );

            return Result<Tag>.FromSuccess( tag );
        }
    }
}

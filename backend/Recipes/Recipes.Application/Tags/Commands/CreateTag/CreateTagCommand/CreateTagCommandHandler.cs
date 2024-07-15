using Recipes.Infrastructure.Repositories;
using Application.Result;
using Application.Validation;
using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.Tags.Commands.CreateTag;

namespace Recipes.Recipes.Infrastructure.Repositories.Tags.Commands.CreateTag
{
    public class CreateTagCommandHandler : ICommandHandler<CreateTagCommand>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IAsyncValidator<CreateTagCommand> _createTagCommandValidator;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTagCommandHandler(
            ITagRepository tagRepository,
            IAsyncValidator<CreateTagCommand> validator,
            IUnitOfWork unitOfWork )
        {
            _tagRepository = tagRepository;
            _createTagCommandValidator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> HandleAsync( CreateTagCommand createTagCommand )
        {
            ValidationResult validationResult = await _createTagCommandValidator.ValidationAsync( createTagCommand );
            if ( validationResult.IsFail )
            {
                return new CommandResult( validationResult );
            }

            Tag tag = new Tag( createTagCommand.Name );

            await _tagRepository.AddAsync( tag );

            await _unitOfWork.CommitAsync();

            return new CommandResult( ValidationResult.Ok() );
        }
    }
}

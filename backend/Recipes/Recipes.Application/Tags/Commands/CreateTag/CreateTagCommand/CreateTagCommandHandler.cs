using Application;
using Application.Result;
using Application.Validation;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Tags.Dtos;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Entities.Tags;

namespace Recipes.Application.Tags.Commands.CreateTag
{
    public class CreateTagCommandHandler : ICommandHandler<CreateTagDto>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IAsyncValidator<CreateTagDto> _createTagCommandValidator;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTagCommandHandler(
            ITagRepository tagRepository,
            IAsyncValidator<CreateTagDto> validator,
            IUnitOfWork unitOfWork )
        {
            _tagRepository = tagRepository;
            _createTagCommandValidator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> HandleAsync( CreateTagDto createTagCommand )
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

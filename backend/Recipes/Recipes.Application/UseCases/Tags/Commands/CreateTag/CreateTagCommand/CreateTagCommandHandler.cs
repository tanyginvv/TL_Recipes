using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.Validation;
using Recipes.Application.Results;
using Recipes.Application.Repositories;

namespace Recipes.Application.UseCases.Tags.Commands.CreateTag.CreateTagCommand
{
    public class CreateTagCommandHandler(
            ITagRepository tagRepository,
            IAsyncValidator<CreateTagCommand> validator,
            IUnitOfWork unitOfWork )
        : ICommandHandler<CreateTagCommand>
    {
        private ITagRepository _tagRepository => tagRepository;
        private IAsyncValidator<CreateTagCommand> _createTagCommandValidator => validator;
        private IUnitOfWork _unitOfWork => unitOfWork;

        public async Task<Result> HandleAsync( CreateTagCommand createTagCommand )
        {
            Result validationResult = await _createTagCommandValidator.ValidationAsync( createTagCommand );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error );
            }

            Tag tag = new Tag( createTagCommand.Name );

            await _tagRepository.AddAsync( tag );

            await _unitOfWork.CommitAsync();

            return Result.Success;
        }
    }
}

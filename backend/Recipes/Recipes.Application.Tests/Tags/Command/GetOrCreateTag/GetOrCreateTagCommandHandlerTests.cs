using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Commands.GetOrCreateTag;
using Recipes.Domain.Entities;
using Recipes.Application.CQRSInterfaces;
using Microsoft.Extensions.Logging;

namespace Recipes.Application.Tests.Tags.Command.GetOrCreateTag;

public class GetOrCreateTagCommandHandlerTests
{
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<IAsyncValidator<GetOrCreateTagCommand>> _validatorMock;
    private readonly GetOrCreateTagCommandHandler _handler;
    private readonly Mock<ILogger<GetOrCreateTagCommand>> _logger;

    public GetOrCreateTagCommandHandlerTests()
    {
        _tagRepositoryMock = new Mock<ITagRepository>();
        _validatorMock = new Mock<IAsyncValidator<GetOrCreateTagCommand>>();
        _logger = new Mock<ILogger<GetOrCreateTagCommand>>();
        _handler = new GetOrCreateTagCommandHandler( _tagRepositoryMock.Object, _validatorMock.Object, _logger.Object );
    }

    [Fact]
    public async Task HandleAsync_TagExists_ShouldReturnExistingTag()
    {
        // Arrange
        GetOrCreateTagCommand command = new GetOrCreateTagCommand { Name = "ExistingTag" };
        Tag existingTag = new Tag( "ExistingTag" );
        _tagRepositoryMock.Setup( repo => repo.GetByNameAsync( command.Name ) )
            .ReturnsAsync( existingTag );
        _validatorMock.Setup( x => x.ValidateAsync( command ) ).ReturnsAsync( Result.FromSuccess );

        // Act
        Result<Tag> result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( existingTag, result.Value );
    }

    [Fact]
    public async Task HandleAsync_TagDoesNotExist_ShouldCreateAndReturnNewTag()
    {
        // Arrange
        GetOrCreateTagCommand command = new GetOrCreateTagCommand { Name = "NewTag" };
        _tagRepositoryMock.Setup( repo => repo.GetByNameAsync( command.Name ) )
            .ReturnsAsync( null as Tag );

        Tag newTag = new Tag( command.Name );
        _tagRepositoryMock.Setup( repo => repo.AddAsync( It.IsAny<Tag>() ) );
        _validatorMock.Setup( x => x.ValidateAsync( command ) ).ReturnsAsync( Result.FromSuccess );

        // Act
        Result<Tag> result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( command.Name, result.Value.Name );
        _tagRepositoryMock.Verify( repo => repo.AddAsync( It.IsAny<Tag>() ), Times.Once );
    }
}
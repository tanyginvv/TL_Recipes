using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Commands.GetOrCreateTag;
using Recipes.Domain.Entities;
using Recipes.Application.CQRSInterfaces;

namespace Recipes.Application.Tests.Tags.Command.GetOrCreateTag;

public class GetOrCreateTagCommandHandlerTests
{
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<IAsyncValidator<GetOrCreateTagCommand>> _validatorMock;
    private readonly GetOrCreateTagCommandHandler _handler;

    public GetOrCreateTagCommandHandlerTests()
    {
        _tagRepositoryMock = new Mock<ITagRepository>();
        _validatorMock = new Mock<IAsyncValidator<GetOrCreateTagCommand>>();
        _handler = new GetOrCreateTagCommandHandler( _tagRepositoryMock.Object, _validatorMock.Object );
    }

    [Fact]
    public async Task HandleImplAsync_Should_Return_Existing_Tag_When_Tag_Exists()
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
    public async Task HandleImplAsync_Should_Create_And_Return_New_Tag_When_Tag_Does_Not_Exist()
    {
        // Arrange
        GetOrCreateTagCommand command = new GetOrCreateTagCommand { Name = "NewTag" };
        _tagRepositoryMock.Setup( repo => repo.GetByNameAsync( command.Name ) )
            .ReturnsAsync( null as Tag  ); // Simulate that tag does not exist

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
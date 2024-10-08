﻿using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.UseCases.Likes.Command.CreateLike;
using Recipes.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Recipes.Application.Tests.Likes.Command.CreateLike;

public class CreateLikeCommandHandlerTests
{
    private readonly Mock<ILikeRepository> _mockLikeRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAsyncValidator<CreateLikeCommand>> _mockValidator;
    private readonly CreateLikeCommandHandler _handler;
    private readonly Mock<ILogger<CreateLikeCommand>> _logger;
    public CreateLikeCommandHandlerTests()
    {
        _mockLikeRepository = new Mock<ILikeRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockValidator = new Mock<IAsyncValidator<CreateLikeCommand>>();
        _logger = new Mock<ILogger<CreateLikeCommand>>();
        _handler = new CreateLikeCommandHandler(
            _mockLikeRepository.Object,
            _mockUnitOfWork.Object,
            _mockValidator.Object,
            _logger.Object );
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_AddsLikeAndCommits()
    {
        // Arrange
        CreateLikeCommand command = new CreateLikeCommand { RecipeId = 1, UserId = 2 };
        _mockValidator.Setup( v => v.ValidateAsync( command ) )
                      .ReturnsAsync( Result.Success ); 

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _mockLikeRepository.Verify( r => r.AddAsync( It.Is<Like>( l => l.RecipeId == command.RecipeId && l.UserId == command.UserId ) ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Once );
        Assert.True( result.IsSuccess );
        Assert.Null( result.Error );
    }

    [Fact]
    public async Task HandleAsync_InvalidCommand_ReturnsValidationError()
    {
        // Arrange
        CreateLikeCommand command = new CreateLikeCommand { RecipeId = 1, UserId = 2 };
        _mockValidator.Setup( v => v.ValidateAsync( command ) )
                      .ReturnsAsync( Result.FromError( "Validation error" ) );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _mockLikeRepository.Verify( r => r.AddAsync( It.IsAny<Like>() ), Times.Never ); 
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never ); 
        Assert.False( result.IsSuccess );
        Assert.Equal( "Validation error", result.Error.Message );
    }

    [Fact]
    public async Task HandleAsync_ExceptionThrown_ReturnsError()
    {
        // Arrange
        CreateLikeCommand command = new CreateLikeCommand { RecipeId = 1, UserId = 2 };
        _mockValidator.Setup( v => v.ValidateAsync( command ) )
                      .ReturnsAsync( Result.Success );

        _mockLikeRepository.Setup( r => r.AddAsync( It.IsAny<Like>() ) )
                           .ThrowsAsync( new System.Exception( "Repository error" ) ); 

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never );
        Assert.False( result.IsSuccess );
        Assert.Equal( "Repository error", result.Error.Message );
    }
}
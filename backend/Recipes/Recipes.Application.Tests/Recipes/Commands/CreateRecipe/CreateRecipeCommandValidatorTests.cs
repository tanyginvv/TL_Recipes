using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Commands.CreateRecipe;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Recipes.Commands.CreateRecipe;

public class CreateRecipeCommandValidatorTests
{
    private readonly IAsyncValidator<CreateRecipeCommand> _validator;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly User _existingUser;

    public CreateRecipeCommandValidatorTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _validator = new CreateRecipeCommandValidator( _mockUserRepository.Object );
        _existingUser = new User( "test", "test", "test" );
        _mockUserRepository.Setup( repo => repo.GetByIdAsync( _existingUser.Id ) ).ReturnsAsync( _existingUser );
    }

    [Fact]
    public async Task ValidateAsync_UserDoesNotExist_ReturnsError()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = 2, 
            Name = "Valid Recipe",
            Description = "A valid description.",
            PortionCount = 1,
            CookTime = 30,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto>(),
            Steps = new List<StepDto>()
        };

        _mockUserRepository.Setup( repo => repo.GetByIdAsync( command.AuthorId ) ).ReturnsAsync( null as User  );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Такого пользователя не существует", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_NameIsNull_ReturnsError()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = _existingUser.Id,
            Name = null,
            Description = "A valid description.",
            PortionCount = 1,
            CookTime = 30,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto>(),
            Steps = new List<StepDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название блюда не может быть пустым", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_NameTooLong_ReturnsError()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = _existingUser.Id,
            Name = new string( 'a', 101 ),
            Description = "A valid description.",
            PortionCount = 1,
            CookTime = 30,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto>(),
            Steps = new List<StepDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название блюда не может быть больше чем 100 символов", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_DescriptionIsNull_ReturnsError()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = _existingUser.Id,
            Name = "Valid Recipe",
            Description = null,
            PortionCount = 1,
            CookTime = 30,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto>(),
            Steps = new List<StepDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Описание блюда не может быть пустым", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_DescriptionTooLong_ReturnsError()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = _existingUser.Id,
            Name = "Valid Recipe",
            Description = new string( 'a', 151 ), 
            PortionCount = 1,
            CookTime = 30,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto>(),
            Steps = new List<StepDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Описание блюда не может быть больше чем 150 символов", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_PortionCountIsZero_ReturnsError()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = _existingUser.Id,
            Name = "Valid Recipe",
            Description = "A valid description.",
            PortionCount = 0,
            CookTime = 30,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto>(),
            Steps = new List<StepDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Количество порций должно быть больше 0", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_CookTimeIsZero_ReturnsError()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = _existingUser.Id,
            Name = "Valid Recipe",
            Description = "A valid description.",
            PortionCount = 1,
            CookTime = 0,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto>(),
            Steps = new List<StepDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Время приготовления должно быть больше 0", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ImageUrlIsNull_ReturnsError()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = _existingUser.Id,
            Name = "Valid Recipe",
            Description = "A valid description.",
            PortionCount = 1,
            CookTime = 30,
            ImageUrl = null,
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto>(),
            Steps = new List<StepDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Изображение блюда должно быть обязательно", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_TagsCountExceedsLimit_ReturnsError()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = _existingUser.Id,
            Name = "Valid Recipe",
            Description = "A valid description.",
            PortionCount = 1,
            CookTime = 30,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>
            {
                new TagDto(), new TagDto(), new TagDto(), new TagDto(), new TagDto(), new TagDto()
            }, // 6 tags
            Ingredients = new List<IngredientDto>(),
            Steps = new List<StepDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Количество тегов ограничено до 5", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_IngredientsEmpty_ReturnsError()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = _existingUser.Id,
            Name = "Valid Recipe",
            Description = "A valid description.",
            PortionCount = 1,
            CookTime = 30,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto>(), 
            Steps = new List<StepDto> { new StepDto { StepDescription = "Step 1" } }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Количество ингредиентов не может быть равно 0", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_StepsEmpty_ReturnsError()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = _existingUser.Id,
            Name = "Valid Recipe",
            Description = "A valid description.",
            PortionCount = 1,
            CookTime = 30,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto> { new IngredientDto { Title = "Ingredient", Description = "Description" } },
            Steps = new List<StepDto>() 
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Количество шагов не может быть равно 0", result.Error.Message );
    }
}
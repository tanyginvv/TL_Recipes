using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Recipes.Commands.UpdateRecipe;

public class UpdateRecipeCommandValidatorTests
{
    private readonly Mock<IRecipeRepository> _mockRecipeRepository;
    private readonly UpdateRecipeCommandValidator _validator;

    public UpdateRecipeCommandValidatorTests()
    {
        _mockRecipeRepository = new Mock<IRecipeRepository>();
        _validator = new UpdateRecipeCommandValidator( _mockRecipeRepository.Object );
    }

    [Fact]
    public async Task ValidateAsync_NameIsNullOrEmpty_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = string.Empty,
            Description = "Some description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto>(),
            Ingredients = new List<IngredientDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название блюда не может быть пустым", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_NameIsTooLong_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = new string( 'a', 101 ),
            Description = "Some description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto>(),
            Ingredients = new List<IngredientDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название блюда не может быть больше чем 100 символов", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_DescriptionIsNullOrEmpty_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Valid Name",
            Description = string.Empty,
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto>(),
            Ingredients = new List<IngredientDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Описание блюда не может быть пустым", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_DescriptionIsTooLong_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Valid Name",
            Description = new string( 'a', 151 ),
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto>(),
            Ingredients = new List<IngredientDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Описание блюда не может быть больше чем 150 символов", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_PortionCountIsInvalid_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Valid Name",
            Description = "Some description",
            CookTime = 30,
            PortionCount = 0,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto>(),
            Ingredients = new List<IngredientDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Количество порций должно быть больше 0", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_CookTimeIsInvalid_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Valid Name",
            Description = "Some description",
            CookTime = 0,
            PortionCount = 4,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto>(),
            Ingredients = new List<IngredientDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Время приготовления должно быть больше 0", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ImageUrlIsNullOrEmpty_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Valid Name",
            Description = "Some description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = string.Empty,
            Tags = new List<TagDto>(),
            Steps = new List<StepDto>(),
            Ingredients = new List<IngredientDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Изображение блюда должно быть обязательно", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_AuthorIdMismatch_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Valid Name",
            Description = "Some description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto>(),
            Ingredients = new List<IngredientDto>()
        };

        Recipe recipe = new Recipe( 1, "Name", "Description", 30, 4, "image_url" ) { AuthorId = 2 };
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( recipe );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "У пользователя нет доступа к обновлению данного рецепта", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_TagsCountExceedsLimit_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Valid Name",
            Description = "Some description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>
            {
                new TagDto { Name = "Tag1" },
                new TagDto { Name = "Tag2" },
                new TagDto { Name = "Tag3" },
                new TagDto { Name = "Tag4" },
                new TagDto { Name = "Tag5" },
                new TagDto { Name = "Tag6" }
            },
            Steps = new List<StepDto>(),
            Ingredients = new List<IngredientDto>()
        };

        Recipe recipe = new Recipe( 1, "Name", "Description", 30, 4, "image_url" ) { AuthorId = 1 };
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( recipe );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Количество тегов ограничено до 5", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_StepsCountIsZero_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Valid Name",
            Description = "Some description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto>(), // No steps
            Ingredients = new List<IngredientDto>()
        };

        Recipe recipe = new Recipe( 1, "Name", "Description", 30, 4, "image_url" ) { AuthorId = 1 };
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( recipe );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Количество шагов не может быть равно 0", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_IngredientsCountIsZero_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Valid Name",
            Description = "Some description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto> { new StepDto() },
            Ingredients = new List<IngredientDto>() // No ingredients
        };

        Recipe recipe = new Recipe( 1, "Name", "Description", 30, 4, "image_url" ) { AuthorId = 1 };
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( recipe );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Количество ингредиентов не может быть равно 0", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Valid Recipe",
            Description = "Valid Description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto> { new StepDto() },
            Ingredients = new List<IngredientDto> { new IngredientDto() }
        };

        Recipe recipe = new Recipe( 1, "Name", "Description", 30, 4, "image_url" ) { AuthorId = 1 };
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( recipe );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
    }
}
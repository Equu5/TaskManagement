using FluentAssertions;

public class CreateTaskCommandValidatorTests
{
    private readonly CreateTaskCommandValidator _validator;

    public CreateTaskCommandValidatorTests()
    {
        _validator = new CreateTaskCommandValidator();
    }

    [Fact]
    public void Validator_ShouldReturnError_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Description = "Test Description",
            AssignedTo = "John Doe",
            Name = null
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateTaskCommand.Name));
    }
}

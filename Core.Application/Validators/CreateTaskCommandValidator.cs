using FluentValidation;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Task name is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Task description is required.");
    }
}

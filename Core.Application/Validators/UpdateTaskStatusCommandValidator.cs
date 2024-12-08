using FluentValidation;

public class UpdateTaskStatusCommandValidator : AbstractValidator<UpdateTaskStatusCommand>
{
    public UpdateTaskStatusCommandValidator()
    {
        RuleFor(x => x.NewStatus).IsInEnum().WithMessage("Invalid status value.");
    }
}

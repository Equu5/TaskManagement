using FluentValidation;

public class GetTaskByIdCommandValidator : AbstractValidator<GetTaskByIdCommand>
{
    public GetTaskByIdCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Task ID must be greater than 0.");
    }
}

using FluentValidation;

namespace MNX.MonitoringCenter.RigsApi.UseCases;

public class RenameRigCommandValidator : AbstractValidator<RenameRigCommand>
{
    public RenameRigCommandValidator()
    {
        RuleFor(model => model.NewName)
            .NotEmpty()
                .WithMessage("Name is required")
            .MinimumLength(3)
                .WithMessage("Name must be at least 3 characters long")
            .MaximumLength(40)
                .WithMessage("Name cannot exceed 40 characters")
            .Matches("^[a-zA-Z0-9_]+")
                .WithMessage("Name must contain only A-Z, a-z characters, digits (0-9) and \"_\"");
    }
}
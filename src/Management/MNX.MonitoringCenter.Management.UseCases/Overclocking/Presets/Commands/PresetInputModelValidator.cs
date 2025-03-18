using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands;

/// <summary>
/// Валидатор <see cref="PresetInputModel"/>.
/// </summary>
public class PresetInputModelValidator : AbstractValidator<PresetInputModel>
{
    public PresetInputModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The Preset Name must not be empty");

        RuleFor(x => x.Overclocking)
            .NotNull()
            .WithMessage("Overclocking is required");
    }
}

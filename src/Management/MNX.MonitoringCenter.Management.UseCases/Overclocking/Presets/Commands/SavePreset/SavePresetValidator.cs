using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.SavePreset;

/// <summary>
/// Валидатор команды сохранения пресета.
/// </summary>
public class SavePresetValidator : AbstractValidator<SavePresetCommand>
{
    public SavePresetValidator()
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Preset model is required")
            .SetValidator(x => new PresetInputModelValidator());
    }
}

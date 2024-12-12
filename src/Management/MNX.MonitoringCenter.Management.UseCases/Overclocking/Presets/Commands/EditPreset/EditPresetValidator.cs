using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.EditPreset;

/// <summary>
/// Валидатор команды редактирования пресета.
/// </summary>
public class EditPresetValidator : AbstractValidator<EditPresetCommand>
{
    public EditPresetValidator()
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Preset model is required")
            .SetValidator(x => new PresetInputModelValidator());
    }
}

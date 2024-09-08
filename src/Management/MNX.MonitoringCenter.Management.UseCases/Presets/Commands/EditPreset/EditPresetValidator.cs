using FluentValidation;
using MNX.MonitoringCenter.Management.UseCases.Presets.Commands;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.EditPreset;

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

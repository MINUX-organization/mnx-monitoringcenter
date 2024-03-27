using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;

/// <summary>
/// Валидатор команды обновления пресета
/// </summary>
public class UpdatePresetValidator : AbstractValidator<UpdatePresetCommand>
{
    public UpdatePresetValidator()
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Preset data is required")
            .SetValidator(x => new PresetModelValidator());
    }
}

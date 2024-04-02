using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

/// <summary>
/// Валидатор команды сохранения пресета
/// </summary>
public class SavePresetValidator : AbstractValidator<SavePresetCommand>
{
    public SavePresetValidator()
    {
        RuleFor(x => x.SavePresetModel.GpuName)
            .NotEmpty()
            .WithMessage("The GPU name must not be empty");

        RuleFor(x => x.SavePresetModel.PresetModel)
            .NotNull()
            .WithMessage("Preset data is required")
            .SetValidator(x => new PresetModelValidator());
    }
}

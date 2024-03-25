using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

/// <summary>
/// Валидатор каманды сохранения пресета
/// </summary>
public class SavePresetValidator : AbstractValidator<SavePresetCommand>
{
    public SavePresetValidator()
    {
        RuleFor(x => x.SavePresetModel.GpuName)
            .NotEmpty()
            .WithMessage("Название GPU не должно быть пустым");

        RuleFor(x => x.SavePresetModel.PresetModel)
            .NotNull()
            .WithMessage("Данные для пресета обязательны")
            .SetValidator(x => new PresetModelValidator());
    }
}

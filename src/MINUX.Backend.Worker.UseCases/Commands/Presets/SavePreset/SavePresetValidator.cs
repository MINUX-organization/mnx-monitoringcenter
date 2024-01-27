using FluentValidation;
using MINUX.Backend.Worker.UseCases.Commands.ModelValidators;

namespace MINUX.Backend.Worker.UseCases.Commands.Presets.SavePreset;

/// <summary>
/// Валидатор каманды сохранения пресета
/// </summary>
public class SavePresetValidator : AbstractValidator<SavePresetCommand>
{
    public SavePresetValidator()
    {
        RuleFor(x => x.GpuName)
            .NotEmpty()
            .WithMessage("Название GPU не должно быть пустым");

        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Данные для пресета обязательны")
            .SetValidator(x => new PresetModelValidator());
    }
}

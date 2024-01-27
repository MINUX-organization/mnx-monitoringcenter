using FluentValidation;
using MINUX.Backend.Worker.UseCases.Commands.ModelValidators;

namespace MINUX.Backend.Worker.UseCases.Commands.Presets.UpdatePreset;

/// <summary>
/// Валидатор команды обновления пресета
/// </summary>
public class UpdatePresetValidator : AbstractValidator<UpdatePresetCommand>
{
    public UpdatePresetValidator()
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Данные для пресета обязательны")
            .SetValidator(x => new PresetModelValidator());
    }
}

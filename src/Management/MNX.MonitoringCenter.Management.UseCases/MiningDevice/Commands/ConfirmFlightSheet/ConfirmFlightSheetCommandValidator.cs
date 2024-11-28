using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.MiningDevice.Commands.SetFlightSheet;

/// <summary>
/// Валидатор команды установки полётного листа.
/// </summary>
public class ConfirmFlightSheetCommandValidator : AbstractValidator<ConfirmFlightSheetCommand>
{
    public ConfirmFlightSheetCommandValidator()
    {
        RuleFor(x => x.MiningDevices)
            .NotNull()
            .NotEmpty()
            .WithMessage("Mining devices are required");
    }
}

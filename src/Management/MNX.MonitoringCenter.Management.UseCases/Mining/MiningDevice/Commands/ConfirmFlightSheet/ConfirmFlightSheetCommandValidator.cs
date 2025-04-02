using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.ConfirmFlightSheet;

/// <summary>
/// Валидатор команды установки полётного листа.
/// </summary>
public class ConfirmFlightSheetCommandValidator : AbstractValidator<ConfirmFlightSheetCommand>
{
    public ConfirmFlightSheetCommandValidator()
    {
        RuleFor(x => x.SuccessfullyMiningDevicesIds)
            .NotNull()
            .WithMessage("Mining devices are required");
    }
}

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
            .WithMessage("Successfull devices are required");

        RuleFor(x => x.UnsuccessfullyMiningDevicesIds)
            .NotNull()
            .WithMessage("Unsuccessful mining devices are required.");

        RuleFor(x => new { x.SuccessfullyMiningDevicesIds, x.UnsuccessfullyMiningDevicesIds })
            .Must(x => x.SuccessfullyMiningDevicesIds.Length != 0 || x.UnsuccessfullyMiningDevicesIds.Length != 0)
            .WithMessage("At least one list of mining devices must contain values.");
    }
}

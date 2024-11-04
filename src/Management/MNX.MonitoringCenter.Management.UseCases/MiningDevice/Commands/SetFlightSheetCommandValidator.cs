using FluentValidation;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet;

namespace MNX.MonitoringCenter.Management.UseCases.MiningDevice.Commands;

/// <summary>
/// Валидатор команды установки полётного листа.
/// </summary>
public class SetFlightSheetCommandValidator : AbstractValidator<SetFlightSheetCommand>
{
    public SetFlightSheetCommandValidator(IFlightSheetRepository flightSheetRepository)
    {
        RuleFor(x => x.MiningDevices)
            .NotNull()
            .NotEmpty()
            .WithMessage("Mining devices are required");

        RuleFor(x => x.FightSheetId)
            .MustAsync(flightSheetRepository.Exists)
            .WithMessage("Flight sheet was`t found");
    }
}

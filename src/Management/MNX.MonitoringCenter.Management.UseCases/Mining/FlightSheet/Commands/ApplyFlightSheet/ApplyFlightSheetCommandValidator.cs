using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.ApplyFlightSheet;

/// <summary>
/// Валидатор команды применения полётного листа.
/// </summary>
public class ApplyFlightSheetCommandValidator : AbstractValidator<ApplyFlightSheetCommand>
{
    public ApplyFlightSheetCommandValidator()
    {
        RuleFor(x => x.MiningDevices)
            .NotNull()
            .WithMessage("Mining devices are required");
    }
}

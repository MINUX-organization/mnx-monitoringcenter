using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.ApplyFlightSheet;

/// <summary>
/// Валидатор команды применения полётного листа.
/// </summary>
public class ApplyFlightSheetCommandValidator : AbstractValidator<ApplyFlightSheetCommand>
{ 
    public ApplyFlightSheetCommandValidator()
    {
        RuleFor(x => x.MiningDevices)
            .NotNull()
            .NotEmpty()
            .WithMessage("Mining devices are required");
    }
}

using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.MiningDevice.Commands.SetRigDevices;

/// <summary>
/// Валидатор команды установки устройств на риг.
/// </summary>
public class SetDevicesRigCommandValidator : AbstractValidator<SetRigDevicesCommand>
{
    public SetDevicesRigCommandValidator()
    {
        RuleFor(x => x.RigId)
            .NotEmpty()
            .WithMessage("Rig id is required");

        RuleFor(x => x.RigOwnerId)
            .NotEmpty()
            .WithMessage("Rig owner id is required");
    }
}

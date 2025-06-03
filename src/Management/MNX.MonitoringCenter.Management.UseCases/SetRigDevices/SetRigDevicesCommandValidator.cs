using FluentValidation;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices.DevicesValidation.CpuModelValidation;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices.DevicesValidation.GpuModelValidation;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

/// <summary>
/// Валидатор команды установки устройств на риг.
/// </summary>
public class SetRigDevicesCommandValidator : AbstractValidator<SetRigDevicesCommand>
{
    public SetRigDevicesCommandValidator()
    {
        RuleFor(x => x.RigId)
            .NotEmpty()
            .WithMessage("Rig id is required");

        RuleFor(x => x.RigOwnerId)
            .NotEmpty()
            .WithMessage("Rig owner id is required");

        RuleFor(x => x.Gpus)
            .NotNull()
                .WithMessage($"{nameof(SetRigDevicesCommand.Gpus)} must not be null")
            .ForEach(x => x.SetValidator(new GpuModelValidator()));

        RuleFor(x => x.Cpus)
            .NotNull()
            .NotEmpty()
                .WithMessage($"{nameof(SetRigDevicesCommand.Cpus)} is required")
            .ForEach(x => x.SetValidator(new CpuModelValidator()));
    }
}

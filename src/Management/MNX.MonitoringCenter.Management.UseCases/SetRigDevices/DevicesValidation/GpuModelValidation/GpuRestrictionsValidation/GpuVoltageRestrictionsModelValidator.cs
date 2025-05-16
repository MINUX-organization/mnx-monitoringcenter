using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices.DevicesValidation.GpuModelValidation.GpuRestrictionsValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="GpuVoltageRestrictions"/>
/// </summary>
public class GpuVoltageRestrictionsModelValidator : AbstractValidator<GpuVoltageRestrictions>
{
    public GpuVoltageRestrictionsModelValidator()
    {
        RuleFor(model => model.Core)
            .NotNull()
                .WithMessage($"{nameof(GpuVoltageRestrictions.Core)} is required")
            .SetValidator(new GpuChangingValueModelValidator());

        RuleFor(model => model.Memory)
            .NotNull()
                .WithMessage($"{nameof(GpuVoltageRestrictions.Memory)} is required")
            .SetValidator(new GpuChangingValueModelValidator());
    }
}

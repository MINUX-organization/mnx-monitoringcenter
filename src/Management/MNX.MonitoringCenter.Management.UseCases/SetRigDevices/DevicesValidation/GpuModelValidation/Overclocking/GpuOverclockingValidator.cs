using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices.DevicesValidation.GpuModelValidation.Overclocking;

/// <summary>
/// Валидатор параметров сущности <see cref="GpuOverclocking"/>.
/// </summary>
public class GpuOverclockingValidator : AbstractValidator<GpuOverclocking>
{
    ///
    public GpuOverclockingValidator()
    {
        RuleFor(model => model.PowerLimit)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(GpuOverclocking.PowerLimit)} cannot be negative");

        RuleFor(model => model.FanSpeed)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(GpuOverclocking.FanSpeed)} cannot be negative");
    }
}

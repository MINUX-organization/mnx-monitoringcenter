using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices.DevicesValidation.GpuModelValidation.GpuInformationValidation;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices.DevicesValidation.GpuModelValidation.GpuOverclockingValidation;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices.DevicesValidation.GpuModelValidation.GpuRestrictionsValidation;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices.DevicesValidation.GpuModelValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="Gpu"/>.
/// </summary>
public class GpuModelValidator : AbstractValidator<Gpu>
{
    public GpuModelValidator()
    {
        RuleFor(model => model.Pci)
            .NotNull()
                .WithMessage($"{nameof(Gpu.Pci)} is required")
            .SetValidator(new PciModelValidator());

        RuleFor(model => model.Information)
            .NotNull()
                .WithMessage("Gpu information is required")
            .SetValidator(new GpuInformationValidator());

        RuleFor(model => model.Restrictions)
            .NotNull()
                .WithMessage("Gpu restrictions is required")
            .SetValidator(new GpuRestrictionsModelValidator());

        RuleFor(model => model.Overclocking)
            .NotNull()
                .WithMessage("Gpu overclocking is required")
            .SetValidator(new GpuOverclockingModelValidator());
    }
}
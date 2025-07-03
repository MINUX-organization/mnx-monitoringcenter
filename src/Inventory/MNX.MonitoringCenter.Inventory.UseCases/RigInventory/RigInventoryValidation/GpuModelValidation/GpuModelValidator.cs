using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Information;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.GpuModelValidation.Overclocking;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.GpuModelValidation.Restrictions;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.GpuModelValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="Gpu"/>.
/// </summary>
public class GpuModelValidator : AbstractValidator<Gpu>
{
    ///
    public GpuModelValidator()
    {
        RuleFor(model => model.Pci)
            .NotNull()
            .WithMessage($"{nameof(Gpu.Pci)} is required")
            .SetValidator(new PciModelValidator());

        RuleFor(model => model.Information)
            .NotNull()
            .WithMessage("Gpu information is required")
            .DependentRules(() =>
            {
                RuleFor(model => model.Information.Manufacturer)
                    .Must(name => !string.IsNullOrEmpty(name))
                    .WithMessage($"{nameof(GpuInformation.Manufacturer)} is required");

                RuleFor(model => model.Information.Model)
                    .Must(name => !string.IsNullOrEmpty(name))
                    .WithMessage($"{nameof(Gpu.Information.Model)} is required");

                RuleFor(model => model.Information.Technology)
                    .NotNull()
                    .WithMessage($"{nameof(GpuInformation.Technology)} is required");

                RuleFor(model => model.Information.Memory)
                    .NotNull()
                    .WithMessage($"{nameof(GpuInformation.Memory)} is required");

                RuleFor(model => model.Information.Memory.Total)
                    .GreaterThan(0)
                    .WithMessage($"{nameof(MemoryInformation.Total)} cannot be zero or negative");
            });

        RuleFor(model => model.Restrictions)
            .NotNull()
            .WithMessage($"{nameof(GpuRestrictions)} is required")
            .SetValidator(new GpuRestrictionsValidator())
            .SetInheritanceValidator(validator =>
            {
                validator.Add(new NvidiaRestrictionsValidator());
                validator.Add(new AmdRestrictionsValidator());
            });

        RuleFor(model => model.Overclocking)
            .NotNull()
            .WithMessage($"{nameof(GpuOverclocking)} is required")
            .SetValidator(new GpuOverclockingValidator())
            .SetInheritanceValidator(validator =>
            {
                validator.Add(new NvidiaOverclockingValidator());
                validator.Add(new AmdOverclockingValidator());
            });
    }
}
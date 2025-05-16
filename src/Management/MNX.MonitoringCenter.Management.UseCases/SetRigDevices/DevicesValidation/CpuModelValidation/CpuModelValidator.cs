using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices.DevicesValidation.CpuModelValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="Cpu"/>.
/// </summary>
public class CpuModelValidator : AbstractValidator<Cpu>
{
    public CpuModelValidator()
    {
        RuleFor(model => model.Pci)
            .NotNull()
                .WithMessage($"{nameof(Cpu.Pci)} is required")
            .SetValidator(new PciModelValidator());

        RuleFor(model => model.Information)
            .NotNull()
                .WithMessage($"{nameof(Cpu.Information)} is required")
            .SetValidator(new CpuInformationModelValidator());

        RuleFor(model => model.Restrictions)
            .NotNull()
                .WithMessage($"{nameof(Cpu.Restrictions)} is required")
            .SetValidator(new CpuRestrictionsModelValidator());

        RuleFor(model => model.Overclocking)
            .NotNull()
                .WithMessage($"{nameof(Cpu.Overclocking)} is required")
            .SetValidator(new CpuOverclockingModelValidator());
    }
}
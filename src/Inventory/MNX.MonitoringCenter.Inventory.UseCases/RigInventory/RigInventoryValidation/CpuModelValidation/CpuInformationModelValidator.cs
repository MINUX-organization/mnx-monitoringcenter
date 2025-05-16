using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.CpuModelValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="CpuInformation"/>.
/// </summary>
public class CpuInformationModelValidator : AbstractValidator<CpuInformation>
{
    public CpuInformationModelValidator()
    {
        RuleFor(model => model.Manufacturer)
            .Must(value => !string.IsNullOrEmpty(value))
                .WithMessage("Manufacturer is required");

        RuleFor(model => model.Model)
            .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("Model of is required");

        RuleFor(model => model.CoresCount)
            .GreaterThan(0)
                .WithMessage("Cores count cannot be less than 1");

        RuleFor(model => model.ThreadsCount)
            .GreaterThan(0)
                .WithMessage("Threads count cannot be less than 1");

        RuleFor(model => model.Architecture)
            .Must(value => !string.IsNullOrEmpty(value))
                .WithMessage("Architecture is required");

        RuleFor(model => model.Cache)
            .NotNull()
                .WithMessage("Cpu cache is required");
    }
}
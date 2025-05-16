using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Information;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices.DevicesValidation.GpuModelValidation.GpuInformationValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="GpuInformation"/>.
/// </summary>
public class GpuInformationValidator : AbstractValidator<GpuInformation>
{
    public GpuInformationValidator()
    {
        RuleFor(model => model.Manufacturer)
            .Must(name => !string.IsNullOrEmpty(name))
                .WithMessage($"{nameof(Gpu.Information.Manufacturer)} is required");

        RuleFor(model => model.Model)
            .Must(name => !string.IsNullOrEmpty(name))
                .WithMessage($"{nameof(Gpu.Information.Model)} is required");

        RuleFor(model => model.Technology)
            .NotNull()
                .WithMessage($"{nameof(GpuInformation.Technology)} is required");

        RuleFor(model => model.Memory)
            .NotNull()
                .WithMessage($"{nameof(GpuInformation.Memory)} is required");
        RuleFor(model => model.Memory.Total)
            .GreaterThan(0)
                .WithMessage($"{nameof(MemoryInformation.Total)} cannot be zero or negative");
    }
}
using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices.DevicesValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="Pci"/>.
/// </summary>
public class PciModelValidator : AbstractValidator<Pci>
{
    public PciModelValidator()
    {
        RuleFor(model => model.Bus)
            .Must(value => !string.IsNullOrEmpty(value))
                .WithMessage($"{nameof(Gpu.Pci.Bus)} is required")
            .Must(BeEqualToIdInDecimal)
                .WithMessage($"{nameof(Gpu.Pci.Bus)} and {nameof(Gpu.Pci.Id)} do not match");
    }

    private bool BeEqualToIdInDecimal(Pci pci, string bus)
    {
        if (string.IsNullOrEmpty(bus))
            return false;

        var hexPart = bus[..2];
        if (hexPart.Length == 0)
            return false;

        if (int.TryParse(hexPart, System.Globalization.NumberStyles.HexNumber, null, out int busValue))
        {
            return busValue == pci.Id;
        }

        return false;
    }
}

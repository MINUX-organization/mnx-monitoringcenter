using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="RigInventoryModel"/>.
/// </summary>
public class RigInventoryModelValidator : AbstractValidator<RigInventoryModel>
{
    public RigInventoryModelValidator()
    {
        RuleFor(model => model)
            .Must(HaveAllOccupiedSlotsMatchedByDevices)
                .WithMessage("Each occupied PCI slot on the motherboard must be matched by a device with the same Bus");
    }

    private bool HaveAllOccupiedSlotsMatchedByDevices(RigInventoryModel model)
    {
        if (model.Motherboard is null) return false;

        var occupiedBuses = model.Motherboard.Pcies
            .Where(slot => slot.IsInstalled)
            .Select(slot => slot.Bus)
            .ToHashSet(StringComparer.InvariantCultureIgnoreCase);

        var deviceBuses = model.Cpus
            .Where(cpu => cpu.Pci != null)
            .Select(cpu => cpu.Pci.Bus)
            .Concat(model.Gpus
                .Where(gpu => gpu.Pci != null)
                .Select(gpu => gpu.Pci.Bus))
            .ToHashSet(StringComparer.InvariantCultureIgnoreCase);

        bool isCpu00Present = model.Cpus.Any(cpu => cpu.Pci?.Bus == "00:00.0");
        bool isGpu00Present = model.Gpus.Any(gpu => gpu.Pci?.Bus == "00:00.0");
        if (isGpu00Present || !isCpu00Present) return false;

        return occupiedBuses.SetEquals(deviceBuses);
    }
}

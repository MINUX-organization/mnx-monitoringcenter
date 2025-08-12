using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.Overclocking.Inventory;

using OverclockingInventory = MonitoringCenter.Inventory.Contracts.Devices.Overclocking;

/// <summary>
/// Оболочка над <see cref="OverclockingInventoryMapperRegistry"/>.
/// Служит для предоставления возможности регистрации универсального, независимого маппера разгона.
/// </summary>
public class OverclockingInventoryMapperRegistryWrapper : IOverclockingInventoryMapper<OverclockingInventory, IOverclocking>
{
    private readonly OverclockingInventoryMapperRegistry _registry;

    ///
    public OverclockingInventoryMapperRegistryWrapper(OverclockingInventoryMapperRegistry registry)
    {
        _registry = registry ?? throw new ArgumentException(nameof(registry));
    }

    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(OverclockingInventory model) => _registry.MapToCore(model);

    /// <inheritdoc/>
    public OverclockingInventory MapToModel(IOverclocking entity) => _registry.MapToModel(entity);
}

using MNX.MonitoringCenter.Inventory.Contracts.Devices;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.OuterModelBuilders.Overclockings;

public abstract class InventoryOverclockingBuilder<TBuilder, TEntity>
    where TBuilder : InventoryOverclockingBuilder<TBuilder, TEntity>
    where TEntity : Overclocking
{
    public abstract Overclocking Build();
}

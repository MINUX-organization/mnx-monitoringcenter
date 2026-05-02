using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.OuterModelBuilders.Overclockings;

public abstract class InventoryGpuOverclockingBuilder<TBuilder, TEntity> :
    InventoryOverclockingBuilder<TBuilder, TEntity>
    where TBuilder : InventoryGpuOverclockingBuilder<TBuilder, TEntity>
    where TEntity : GpuOverclocking
{
    protected int? _powerLimit = null;
    protected int? _fanSpeed = null;

    public TBuilder WithPowerLimit(int powerLimit = 0)
    {
        _powerLimit = powerLimit;
        return (TBuilder)this;
    }

    public TBuilder WithFanSpeed(int fanSpeed = 0)
    {
        _fanSpeed = fanSpeed;
        return (TBuilder)this;
    }

    public abstract override GpuOverclocking Build();
}

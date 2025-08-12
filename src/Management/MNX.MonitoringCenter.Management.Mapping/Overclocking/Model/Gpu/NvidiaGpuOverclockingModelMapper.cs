using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.FanOverclocking.Model;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model.Gpu;

/// <summary>
/// Реализация <see cref="IOverclockingModelMapper{TModel, TEntity}"/> для сущностей
/// <see cref="NvidiaGpuOverclockingModel"/> и <see cref="NvidiaGpuOverclocking"/>.
/// </summary>
public class NvidiaGpuOverclockingModelMapper : IOverclockingModelMapper<NvidiaGpuOverclockingModel, NvidiaGpuOverclocking>
{
    private readonly FanOverclockingModelMapperRegistry _registry;

    ///
    public NvidiaGpuOverclockingModelMapper(FanOverclockingModelMapperRegistry registry)
    {
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
    }

    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(NvidiaGpuOverclockingModel model)
    {
        var fanOverclocking = _registry.MapToCoreEntity(model.FanOverclocking);
        return new NvidiaGpuOverclocking()
        {
            FanOverclocking = fanOverclocking,
            PowerLimit = model.PowerLimit,
            CoreClockLock = model.CoreClockLock,
            CoreClockOffset = model.CoreClockOffset,
            MemoryClockLock = model.MemoryClockLock,
            MemoryClockOffset = model.MemoryClockOffset,
            CoreVoltage = model.CoreVoltage,
            CoreVoltageOffset = model.CoreVoltageOffset,
            MemoryVoltage = model.MemoryVoltage,
            MemoryVoltageOffset = model.MemoryVoltageOffset,
        };
    }

    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(NvidiaGpuOverclockingModel model, NvidiaGpuOverclocking original)
    {
        var fanOverclocking = _registry.MapToCoreEntity(model.FanOverclocking, original.FanOverclocking.Id);
        return new NvidiaGpuOverclocking()
        {
            Id = original.Id,
            FanOverclocking = fanOverclocking,
            PowerLimit = model.PowerLimit,
            CoreClockLock = model.CoreClockLock,
            CoreClockOffset = model.CoreClockOffset,
            MemoryClockLock = model.MemoryClockLock,
            MemoryClockOffset = model.MemoryClockOffset,
            CoreVoltage = model.CoreVoltage,
            CoreVoltageOffset = model.CoreVoltageOffset,
            MemoryVoltage = model.MemoryVoltage,
            MemoryVoltageOffset = model.MemoryVoltageOffset,
        };
    }

    /// <inheritdoc/>
    public IOverclockingModel MapToModel(NvidiaGpuOverclocking entity)
    {
        var fanOverclocking = _registry.MapToModel(entity.FanOverclocking);
        return new NvidiaGpuOverclockingModel()
        {
            FanOverclocking = fanOverclocking,
            PowerLimit = entity.PowerLimit,
            CoreClockLock = entity.CoreClockLock,
            CoreClockOffset = entity.CoreClockOffset,
            MemoryClockLock = entity.MemoryClockLock,
            MemoryClockOffset = entity.MemoryClockOffset,
            CoreVoltage = entity.CoreVoltage,
            CoreVoltageOffset = entity.CoreVoltageOffset,
            MemoryVoltage = entity.MemoryVoltage,
            MemoryVoltageOffset = entity.MemoryVoltageOffset,
        };
    }
}

using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.FanOverclocking.Model;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model.Gpu;

/// <summary>
/// Реализация <see cref="IOverclockingModelMapper{TModel, TEntity}"/> для сущностей
/// <see cref="AmdGpuOverclockingModel"/> и <see cref="AmdGpuOverclocking"/>.
/// </summary>
public class AmdGpuOverclockingModelMapper : IOverclockingModelMapper<AmdGpuOverclockingModel, AmdGpuOverclocking>
{
    private readonly FanOverclockingModelMapperRegistry _registry;

    ///
    public AmdGpuOverclockingModelMapper(FanOverclockingModelMapperRegistry registry)
    {
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
    }

    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(AmdGpuOverclockingModel model)
    {
        var fanOverclocking = _registry.MapToCoreEntity(model.FanOverclocking);
        return new AmdGpuOverclocking()
        {
            FanOverclocking = fanOverclocking,
            PowerLimit = model.PowerLimit,
            CoreClockLock = model.CoreClockLock,
            CoreClockState = model.CoreClockState,
            CoreVoltage = model.CoreVoltage,
            CoreVoltageOffset = model.CoreVoltageOffset,
            MemoryClockLock = model.MemoryClockLock,
            MemoryClockState = model.MemoryClockState,
            MemoryVoltage = model.MemoryVoltage,
            MemoryControllerVoltage = model.MemoryControllerVoltage,
            MemoryTweak = model.MemoryTweak,
            EnhancedOverclock = model.EnhancedOverclock,
            AlternativeDownVoltage = model.AlternativeDownVoltage,
            SocFrequency = model.SocFrequency,
            SocVoltage = model.SocVoltage,
        };
    }

    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(AmdGpuOverclockingModel model, AmdGpuOverclocking original)
    {
        var fanOverclocking = _registry.MapToCoreEntity(model.FanOverclocking, original.FanOverclocking.Id);
        return new AmdGpuOverclocking()
        {
            Id = original.Id,
            FanOverclocking = fanOverclocking,
            PowerLimit = model.PowerLimit,
            CoreClockLock = model.CoreClockLock,
            CoreClockState = model.CoreClockState,
            CoreVoltage = model.CoreVoltage,
            CoreVoltageOffset = model.CoreVoltageOffset,
            MemoryClockLock = model.MemoryClockLock,
            MemoryClockState = model.MemoryClockState,
            MemoryVoltage = model.MemoryVoltage,
            MemoryControllerVoltage = model.MemoryControllerVoltage,
            MemoryTweak = model.MemoryTweak,
            EnhancedOverclock = model.EnhancedOverclock,
            AlternativeDownVoltage = model.AlternativeDownVoltage,
            SocFrequency = model.SocFrequency,
            SocVoltage = model.SocVoltage,
        };
    }

    /// <inheritdoc/>
    public IOverclockingModel MapToModel(AmdGpuOverclocking entity)
    {
        var fanOverclocking = _registry.MapToModel(entity.FanOverclocking);
        return new AmdGpuOverclockingModel()
        {
            FanOverclocking = fanOverclocking,
            PowerLimit = entity.PowerLimit,
            CoreClockLock = entity.CoreClockLock,
            CoreClockState = entity.CoreClockState,
            CoreVoltage = entity.CoreVoltage,
            CoreVoltageOffset = entity.CoreVoltageOffset,
            MemoryClockLock = entity.MemoryClockLock,
            MemoryClockState = entity.MemoryClockState,
            MemoryVoltage = entity.MemoryVoltage,
            MemoryControllerVoltage = entity.MemoryControllerVoltage,
            MemoryTweak = entity.MemoryTweak,
            EnhancedOverclock = entity.EnhancedOverclock,
            AlternativeDownVoltage = entity.AlternativeDownVoltage,
            SocFrequency = entity.SocFrequency,
            SocVoltage = entity.SocVoltage,
        };
    }
}

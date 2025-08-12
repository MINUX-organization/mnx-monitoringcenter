using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model.Cpu;

/// <summary>
/// Реализация <see cref="IOverclockingModelMapper{TModel, TEntity}"/> для сущностей
/// <see cref="CpuOverclockingModel"/> и <see cref="CpuOverclocking"/>.
/// </summary>
public class CpuOverclockingModelMapper : IOverclockingModelMapper<CpuOverclockingModel, CpuOverclocking>
{
    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(CpuOverclockingModel model)
    {
        return new CpuOverclocking
        {
            CoreClockLock = model.CoreClockLock,
            CoreVoltage = model.CoreVoltage
        };
    }

    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(CpuOverclockingModel model, CpuOverclocking original)
    {
        return new CpuOverclocking
        {
            Id = original.Id,
            CoreClockLock = model.CoreClockLock,
            CoreVoltage = model.CoreVoltage
        };
    }

    /// <inheritdoc/>
    public IOverclockingModel MapToModel(CpuOverclocking entity)
    {
        return new CpuOverclockingModel
        {
            CoreClockLock = entity.CoreClockLock,
            CoreVoltage = entity.CoreVoltage
        };
    }
}

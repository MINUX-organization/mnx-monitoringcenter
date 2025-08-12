using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Fan.Model;

/// <summary>
/// Реализация <see cref="IFanOverclockingModelMapper{TModel, TEntity}"/> для сущностей
/// <see cref="FanOverclockingWithTargetTemperatureModel"/> и <see cref="FanOverclockingWithTargetTemperature"/>.
/// </summary>
public class FanTargetTemperatureModelMapper : IFanOverclockingModelMapper<
    FanOverclockingWithTargetTemperatureModel, FanOverclockingWithTargetTemperature>
{
    /// <inheritdoc/>
    public IFanOverclocking MapToCoreEntity(FanOverclockingWithTargetTemperatureModel model)
    {
        return new FanOverclockingWithTargetTemperature()
        {
            MinTargetSpeed = model.MinTargetSpeed,
            MaxTargetSpeed = model.MaxTargetSpeed,
            TargetCoreTemperature = model.TargetCoreTemperature,
            TargetMemoryTemperature = model.TargetMemoryTemperature
        };
    }

    /// <inheritdoc/>
    public IFanOverclocking MapToCoreEntity(FanOverclockingWithTargetTemperatureModel model, Guid originalId)
    {
        return new FanOverclockingWithTargetTemperature()
        {
            Id = originalId,
            MinTargetSpeed = model.MinTargetSpeed,
            MaxTargetSpeed = model.MaxTargetSpeed,
            TargetCoreTemperature = model.TargetCoreTemperature,
            TargetMemoryTemperature = model.TargetMemoryTemperature
        };
    }

    /// <inheritdoc/>
    public IFanOverclockingModel MapToModel(FanOverclockingWithTargetTemperature entity)
    {
        return new FanOverclockingWithTargetTemperatureModel()
        {
            MinTargetSpeed = entity.MinTargetSpeed,
            MaxTargetSpeed = entity.MaxTargetSpeed,
            TargetCoreTemperature = entity.TargetCoreTemperature,
            TargetMemoryTemperature = entity.TargetMemoryTemperature
        };
    }
}

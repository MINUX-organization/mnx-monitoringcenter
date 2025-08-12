using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Fan.Model;

/// <summary>
/// Реализация <see cref="IFanOverclockingModelMapper{TModel, TEntity}"/> для сущностей
/// <see cref="FanOverclockingWithTargetSpeedModel"/> и <see cref="FanOverclockingWithTargetSpeed"/>.
/// </summary>
public class FanTargetSpeedModelMapper : IFanOverclockingModelMapper<
    FanOverclockingWithTargetSpeedModel, FanOverclockingWithTargetSpeed>
{
    /// <inheritdoc/>
    public IFanOverclocking MapToCoreEntity(FanOverclockingWithTargetSpeedModel model)
    {
        return new FanOverclockingWithTargetSpeed()
        {
            TargetSpeed = model.TargetSpeed
        };
    }

    /// <inheritdoc/>
    public IFanOverclocking MapToCoreEntity(FanOverclockingWithTargetSpeedModel model, Guid originalId)
    {
        return new FanOverclockingWithTargetSpeed()
        {
            Id = originalId,
            TargetSpeed = model.TargetSpeed
        };
    }

    /// <inheritdoc/>
    public IFanOverclockingModel MapToModel(FanOverclockingWithTargetSpeed entity)
    {
        return new FanOverclockingWithTargetSpeedModel()
        {
            TargetSpeed = entity.TargetSpeed
        };
    }
}

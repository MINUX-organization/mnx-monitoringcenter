using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Fan.Model;

/// <summary>
/// Реализация <see cref="IFanOverclockingModelMapper{TModel, TEntity}"/> для сущностей
/// <see cref="FanOverclockingWithLinearDependenceModel"/> и <see cref="FanOverclockingWithLinearDependence"/>.
/// </summary>
public class FanLinearDependenceModelMapper : IFanOverclockingModelMapper<
    FanOverclockingWithLinearDependenceModel, FanOverclockingWithLinearDependence>
{
    /// <inheritdoc/>
    public IFanOverclocking MapToCoreEntity(FanOverclockingWithLinearDependenceModel model)
    {
        var index = 0;
        return new FanOverclockingWithLinearDependence()
        {
            TargetPoints = model.TargetPoints.Select(x => new FanGraphicPoint()
            {
                PointIndex = index++,
                FanSpeedValueTarget = x.FanSpeedValueTarget,
                TemperatureValueTarget = x.TemperatureValueTarget
            }).ToArray()
        };
    }

    /// <inheritdoc/>
    public IFanOverclocking MapToCoreEntity(FanOverclockingWithLinearDependenceModel model, Guid originalId)
    {
        var index = 0;
        return new FanOverclockingWithLinearDependence()
        {
            Id = originalId,
            TargetPoints = model.TargetPoints.Select(x => new FanGraphicPoint()
            {
                PointIndex = index++,
                FanSpeedValueTarget = x.FanSpeedValueTarget,
                TemperatureValueTarget = x.TemperatureValueTarget
            }).ToArray()
        };
    }

    /// <inheritdoc/>
    public IFanOverclockingModel MapToModel(FanOverclockingWithLinearDependence entity)
    {
        return new FanOverclockingWithLinearDependenceModel()
        {
            TargetPoints = entity.TargetPoints.Select(x => new FanGraphicPointModel()
            {
                FanSpeedValueTarget = x.FanSpeedValueTarget,
                TemperatureValueTarget = x.TemperatureValueTarget
            }).ToArray()
        };
    }
}

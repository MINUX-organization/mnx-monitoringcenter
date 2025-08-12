using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model.Gpu;

/// <summary>
/// Реализация <see cref="IOverclockingModelMapper{TModel, TEntity}"/> для сущностей
/// <see cref="IntelGpuOverclockingModel"/> и <see cref="IntelGpuOverclocking"/>.
/// </summary>
public class IntelGpuOverclockingModelMapper : IOverclockingModelMapper<IntelGpuOverclockingModel, IntelGpuOverclocking>
{
    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(IntelGpuOverclockingModel model)
    {
        return new IntelGpuOverclocking();
    }

    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(IntelGpuOverclockingModel model, IntelGpuOverclocking original)
    {
        return new IntelGpuOverclocking() { Id = original.Id };
    }

    /// <inheritdoc/>
    public IOverclockingModel MapToModel(IntelGpuOverclocking entity)
    {
        return new IntelGpuOverclockingModel();
    }
}

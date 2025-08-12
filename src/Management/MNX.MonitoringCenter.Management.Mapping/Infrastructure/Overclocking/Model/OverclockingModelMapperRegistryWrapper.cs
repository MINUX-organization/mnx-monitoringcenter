using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.Overclocking.Model;

/// <summary>
/// Оболочка над <see cref="OverclockingModelMapperRegistry"/>.
/// Служит для предоставления возможности регистрации универсального, независимого маппера разгона.
/// </summary>
public class OverclockingModelMapperRegistryWrapper : IOverclockingModelMapper<IOverclockingModel, IOverclocking>
{
    private readonly OverclockingModelMapperRegistry _registry;

    ///
    public OverclockingModelMapperRegistryWrapper(OverclockingModelMapperRegistry registry)
    {
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
    }

    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(IOverclockingModel model) => _registry.MapToCoreEntity(model);

    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(IOverclockingModel model, IOverclocking original) => _registry.MapToCoreEntity(model, original);

    /// <inheritdoc/>
    public IOverclockingModel MapToModel(IOverclocking entity) => _registry.MapToModel(entity);
}

using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.Overclocking.Inventory;

using OverclockingInventory = MonitoringCenter.Inventory.Contracts.Devices.Overclocking;

/// <summary>
/// Реализация <see cref="IOverclockingInventoryMappingAdapter"/>.
/// </summary>
public class OverclockingInventoryMappingAdapter<TModel, TEntity> : IOverclockingInventoryMappingAdapter
    where TModel : OverclockingInventory
    where TEntity : IOverclocking
{
    private readonly IOverclockingInventoryMapper<TModel, TEntity> _mapper;

    ///
    public OverclockingInventoryMappingAdapter(IOverclockingInventoryMapper<TModel, TEntity> mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public Type ModelType => typeof(TModel);

    /// <inheritdoc/>
    public Type EntityType => typeof(TEntity);

    /// <inheritdoc/>
    public IOverclocking MapToCore(OverclockingInventory model)
        => _mapper.MapToCoreEntity((TModel)model);

    /// <inheritdoc/>
    public OverclockingInventory MapToModel(IOverclocking entity)
        => _mapper.MapToModel((TEntity)entity);
}

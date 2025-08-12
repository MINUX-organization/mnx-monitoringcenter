using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.FanOverclocking.Model;

/// <summary>
/// Реализация <see cref="IFanOverclockingModelMappingAdapter"/>.
/// </summary>
public class FanOverclockingModelMappingAdapter<TModel, TEntity> : IFanOverclockingModelMappingAdapter
    where TModel : IFanOverclockingModel
    where TEntity : IFanOverclocking
{
    private readonly IFanOverclockingModelMapper<TModel, TEntity> _mapper;

    ///
    public FanOverclockingModelMappingAdapter(IFanOverclockingModelMapper<TModel, TEntity> mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public Type ModelType => typeof(TModel);

    /// <inheritdoc/>
    public Type EntityType => typeof(TEntity);

    /// <inheritdoc/>
    public IFanOverclocking MapToCoreEntity(IFanOverclockingModel model)
        => _mapper.MapToCoreEntity((TModel)model);

    /// <inheritdoc/>
    public IFanOverclocking MapToCoreEntity(IFanOverclockingModel model, Guid originalId)
        => _mapper.MapToCoreEntity((TModel)model, originalId);

    /// <inheritdoc/>
    public IFanOverclockingModel MapToModel(IFanOverclocking entity)
        => _mapper.MapToModel((TEntity)entity);
}

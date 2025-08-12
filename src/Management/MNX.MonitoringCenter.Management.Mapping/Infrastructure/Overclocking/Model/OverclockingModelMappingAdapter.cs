using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.Overclocking.Model;

/// <summary>
/// Реализация <see cref="IOverclockingModelMappingAdapter"/>.
/// </summary>
public class OverclockingModelMappingAdapter<TModel, TEntity> : IOverclockingModelMappingAdapter
    where TModel : IOverclockingModel
    where TEntity : IOverclocking
{
    private readonly IOverclockingModelMapper<TModel, TEntity> _mapper;

    ///
    public OverclockingModelMappingAdapter(IOverclockingModelMapper<TModel, TEntity> mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(IOverclockingModel model)
        => _mapper.MapToCoreEntity((TModel)model);

    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(IOverclockingModel model, IOverclocking original)
        => _mapper.MapToCoreEntity((TModel)model, (TEntity)original);

    /// <inheritdoc/>
    public IOverclockingModel MapToModel(IOverclocking entity)
        => _mapper.MapToModel((TEntity)entity);

    /// <inheritdoc/>
    public Type ModelType => typeof(TModel);

    /// <inheritdoc/>
    public Type EntityType => typeof(TEntity);
}

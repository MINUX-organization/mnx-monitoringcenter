using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Agent;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.FanOverclocking.Agent;

using FanOverclockingAgent = Management.Agent.Commands.Overclocking.Fan.FanOverclocking;

/// <summary>
/// Реализация <see cref="IFanOverclockingAgentMappingAdapter"/>.
/// </summary>
public class FanOverclockingAgentMappingAdapter<TModel, TEntity> : IFanOverclockingAgentMappingAdapter
    where TModel : FanOverclockingAgent
    where TEntity : IFanOverclocking
{
    private readonly IFanOverclockingAgentMapper<TModel, TEntity> _mapper;

    ///
    public FanOverclockingAgentMappingAdapter(IFanOverclockingAgentMapper<TModel, TEntity> mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public Type ModelType => typeof(TModel);

    /// <inheritdoc/>
    public Type EntityType => typeof(TEntity);

    /// <inheritdoc/>
    public FanOverclockingAgent MapToModel(IFanOverclocking entity)
        => _mapper.MapToModel((TEntity)entity);
}

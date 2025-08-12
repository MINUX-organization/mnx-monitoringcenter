using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Agent;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.Overclocking.Agent;

using FanOverclockingAgent = Management.Agent.Commands.Overclocking.Fan.FanOverclocking;

/// <summary>
/// Оболочка над <see cref="OverclockingToFanOverclockingAgentMapper{TEntity, TModel}"/>.
/// Служит для предоставления возможности регистрации универсального, независимого маппера разгона.
/// </summary>
/// <КОСТЫЛИЩЕ>
/// Обертка над <see cref="OverclockingToFanOverclockingAgentMapper{TEntity, TModel}"/>,
/// необходимая для регистрации неуниверсального маппера, как универсальный.
/// </КОСТЫЛИЩЕ>
public class OverclockingAgentMapperWrapper : IOverclockingToFanOverclockingAgentMapper<IOverclocking, FanOverclockingAgent>
{
    private readonly OverclockingToFanOverclockingAgentMapper _mapper;

    ///
    public OverclockingAgentMapperWrapper(OverclockingToFanOverclockingAgentMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public FanOverclockingAgent MapToFanOverclockingModel(IOverclocking entity)
        => _mapper.MapToFanOverclockingModel(entity);
}

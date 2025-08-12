using MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;
using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking;

/// <summary>
/// Интерфейс маппера реализаций сущностей <see cref="IOverclocking"/>
/// и <see cref="FanOverclocking"/>.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TModel"></typeparam>
public interface IOverclockingToFanOverclockingAgentMapper<TEntity, TModel>
    where TEntity : IOverclocking
    where TModel : FanOverclocking
{
    /// <summary>
    /// Преобразовать реализации сущности <see cref="IOverclocking"/>
    /// в реализацию сущности <see cref="FanOverclocking"/>
    /// </summary>
    /// <param name="entity"> Модель данных. </param>
    /// <returns>
    /// Новый экземпляр реализации <see cref="FanOverclocking"/>.
    /// </returns>
    FanOverclocking MapToFanOverclockingModel(IOverclocking entity);
}

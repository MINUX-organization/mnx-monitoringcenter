using MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;
using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Agent;

/// <summary>
/// Интерфейс маппера реализации сущностей
/// <see cref="FanOverclocking"/> и <see cref="IFanOverclocking"/>.
/// </summary>
/// <typeparam name="TModel"> Сущности контрактов агента <see cref="FanOverclocking"/>. </typeparam>
/// <typeparam name="TEntity"> Сущности ядра <see cref="IFanOverclocking"/>. </typeparam>
public interface IFanOverclockingAgentMapper<TModel, TEntity>
    where TModel : FanOverclocking
    where TEntity : IFanOverclocking
{
    /// <summary>
    /// Преобразовать реализацию сущности <see cref="IFanOverclocking"/>
    /// в реализацию сущности <see cref="FanOverclocking"/>.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns> Новый экземпляр <see cref="FanOverclocking"/>. </returns>
    FanOverclocking MapToModel(TEntity entity);
}

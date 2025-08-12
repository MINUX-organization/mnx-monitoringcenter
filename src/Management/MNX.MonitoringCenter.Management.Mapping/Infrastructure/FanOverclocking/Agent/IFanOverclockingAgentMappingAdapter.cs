using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.FanOverclocking.Agent;

using FanOverclockingAgent = Management.Agent.Commands.Overclocking.Fan.FanOverclocking;

/// <summary>
/// Адаптер для полиморфного маппера Сущностей <see cref="IFanOverclocking"/> и <see cref="FanOverclockingAgent"/>.
/// </summary>
public interface IFanOverclockingAgentMappingAdapter
{
    /// <summary>
    /// Преобразовать модель <see cref="IFanOverclocking"/> в сущность <see cref="FanOverclockingAgent"/>.
    /// </summary>
    /// <param name="entity"> Модель данных. </param>
    /// <returns> Сконструированная сущность <see cref="FanOverclockingAgent"/>. </returns>
    FanOverclockingAgent MapToModel(IFanOverclocking entity);

    /// <summary>
    /// Тип реализации <see cref="IFanOverclocking"/>.
    /// </summary>
    Type ModelType { get; }

    /// <summary>
    /// Тип реализации <see cref="FanOverclockingAgent"/>.
    /// </summary>
    Type EntityType { get; }
}

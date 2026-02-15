using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Events;

/// <summary>
/// Интерфейс маппера сущностей мониторинга в команды агента.
/// </summary>
public interface IAgentCommandsMapper
{
    /// <summary>
    /// Преобразовать коллекцию сущностей <see cref="DeviceFLightSheet"/> в коллекцию <see cref="WorkerSettings"/>.
    /// </summary>
    /// <param name="models"> Модель данных. </param>
    /// <returns> Новая коллекция <see cref="WorkerSettings"/>. </returns>
    List<WorkerSettings> MapToWorkerSettings(List<DeviceFLightSheet> models);
}

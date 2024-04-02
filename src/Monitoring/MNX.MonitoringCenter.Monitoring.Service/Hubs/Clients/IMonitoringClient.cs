using MNX.MonitoringCenter.Monitoring.Contracts.Models;

namespace MNX.MonitoringCenter.Monitoring.Service.Hubs.Clients;

/// <summary>
/// Интерфейс клиента-мониторинга.
/// </summary>
public interface IMonitoringClient
{
    /// <summary>
    /// Получено сообщение состояния ригов.
    /// </summary>
    /// <param name="RigStates"> Список состояния ригов. </param>
    /// <returns></returns>
    Task ReceivedRigsState(List<RigState?> RigStates);

    /// <summary>
    /// Получено сообщение динамических данных ригов.
    /// </summary>
    /// <param name="DynamicData"> Список динамических данных ригов. </param>
    /// <returns></returns>
    Task ReceivedRigsDynamicData(List<RigDynamicData?> DynamicData);

    /// <summary>
    /// Получено сообщение об информации ригов.
    /// </summary>
    /// <param name="message"> Сообщение об информации ригов. </param>
    /// <returns></returns>
    Task ReceivedRigsInformation(IAsyncEnumerable<RigInformation> message);
}

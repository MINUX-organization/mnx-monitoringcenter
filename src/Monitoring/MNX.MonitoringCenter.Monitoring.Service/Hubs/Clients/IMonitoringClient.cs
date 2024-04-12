using MNX.MonitoringCenter.Monitoring.Contracts.Models;
using MNX.MonitoringCenter.Monitoring.Service.Messages;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.ComputeTotalRigsDynamicData.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;

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
    Task ReceivedRigsState(IEnumerable<RigState?> RigStates);

    /// <summary>
    /// Получено сообщение динамических данных ригов.
    /// </summary>
    /// <param name="DynamicData"> Список динамических данных ригов. </param>
    Task ReceivedRigsDynamicData(IEnumerable<RigDynamicData?> DynamicData);

    /// <summary>
    /// Получено сообщение об информации ригов.
    /// </summary>
    /// <param name="message"> Сообщение об информации ригов. </param>
    Task ReceivedRigsInformation(IEnumerable<RigInformationMessage> message);

    /// <summary>
    /// Получено сообщение об изменении обобщённых данных.
    /// </summary>
    /// <param name="message"> Сообщение об изменении обобщённых данных. </param>
    Task ReceivedTotalData(TotalDataChangeMessage message);

    /// <summary>
    /// Получена скорость хеширования за период.
    /// </summary>
    /// <param name="message"> Скорость хеширования за период. </param>
    Task ReceivedHashRateForAPeriod(IEnumerable<HashRateModel> message);

    /// <summary>
    /// Получена текущая скорость хеширования.
    /// </summary>
    /// <param name="message"> Текущая скорость хеширования. </param>
    Task ReceivedCurrentHashRate(HashRateModel message);
}

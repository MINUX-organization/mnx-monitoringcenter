using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Models;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Commands.SubscribeClient;

/// <summary>
/// Команда подписки клиента.
/// </summary>
public class SubscribeClientCommand : IStreamRequest<RigInformation>
{
    /// <summary>
    /// Модель подключения.
    /// </summary>
    public ConnectionModel ClientModel { get; set; }

    public SubscribeClientCommand(ConnectionModel clientModel)
    {
        ClientModel = clientModel;
    }
}

using MediatR;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Commands.DisconnectClient;

/// <summary>
/// Команда отключения клиента.
/// </summary>
public class DisconnectClientCommand : IRequest
{
    /// <summary>
    /// Модель подключения.
    /// </summary>
    public ConnectionModel ClientModel { get; set; }

    public DisconnectClientCommand(ConnectionModel clientModel)
    {
        ClientModel = clientModel;
    }
}

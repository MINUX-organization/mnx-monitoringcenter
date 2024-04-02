using EasyNetQ;
using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Enums;
using MNX.MonitoringCenter.Monitoring.Contracts.Messages;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Commands.DisconnectClient;

/// <summary>
/// Обработчик команды отключения клиента.
/// </summary>
public class DisconnectClientCommandHandler : IRequestHandler<DisconnectClientCommand>
{
    private readonly IPubSub _pubSub;

    private readonly ConnectionCounter _connectionCounter;

    public DisconnectClientCommandHandler(IPubSub pubSub, ConnectionCounter connectionCounter)
    {
        _pubSub = pubSub;
        _connectionCounter = connectionCounter;
    }

    public async Task Handle(DisconnectClientCommand request, CancellationToken cancellationToken)
    {
        if (_connectionCounter.RemoveConnectionInGroup(request.ClientModel.UserId) == 0)
        {
            await _pubSub.PublishAsync(new StatisticsStreamStopCommand
            {
                UserId = request.ClientModel.UserId,
                ObservableObjectsType = ObservableObjectsType.Rigs
            }, cancellationToken: cancellationToken);
        }
    }
}

using MediatR;
using EasyNetQ.AutoSubscribe;
using MNX.Application.Bus.RabbitMQ.Agent;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.ConfirmFlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Service.Consumers;

/// <summary>
/// Потребитель сообщений от Агента.
/// </summary>
[AgentMessageConsumer]
public class AgentMsgConsumer :
    IConsumeAsync<RigInventoryMsg>,
    IConsumeAsync<ApplyWorkerSettingsCommandResult>
{
    private readonly IMediator _mediator;

    private readonly IUserRigsObserverAggregator _userRigsObserverAggregator;

    ///
    public AgentMsgConsumer(IMediator mediator, IUserRigsObserverAggregator userRigsObserverAggregator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        _userRigsObserverAggregator = userRigsObserverAggregator
            ?? throw new ArgumentNullException(nameof(userRigsObserverAggregator));
    }

    /// <summary>
    /// Получить сообщение с инвентаризацией.
    /// </summary>
    /// <param name="message"> Сообщение с инвентаризацией. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(RigInventoryMsg message, CancellationToken cancellationToken = default)
    {
        return Task.WhenAll(

            _mediator.Send(new SaveRigInventoryCommand(message), cancellationToken),

            _mediator.Send(new SetRigDevicesCommand(message.RigId, message.RigOwnerId,
                                                    message.Inventory.Gpus, message.Inventory.Cpus),
                cancellationToken)
        );
    }

    /// <summary>
    /// Получить сообщение с результатом применения настроек на воркеры.
    /// </summary>
    /// <param name="message"> Сообщение. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(ApplyWorkerSettingsCommandResult message, CancellationToken cancellationToken = default)
    {
        var confirmFlightSheetCommand = new ConfirmFlightSheetCommand(message.SuccessfullyWorkersIds.ToArray(),
                                                                      message.UnsuccessfullyWorkersIds.ToArray());

        return _mediator.Send(confirmFlightSheetCommand, cancellationToken);
    }
}

using EasyNetQ.AutoSubscribe;
using MediatR;
using MNX.Application.Bus.RabbitMQ.Agent;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.ConfirmFlightSheet;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;
using MNX.MonitoringCenter.RigsApi.UseCases.RigState.Mining;

namespace MNX.MonitoringCenter.RigsApi.Service.Consumers;

/// <summary>
/// Потребитель сообщений от Агента.
/// </summary>
[AgentMessageConsumer]
public class AgentMsgConsumer :
    IConsumeAsync<RigInventoryMsg>,
    IConsumeAsync<StartMiningCommandResult>,
    IConsumeAsync<StopMiningCommandResult>,
    IConsumeAsync<ApplyWorkerSettingsCommandResult>
{
    private readonly IMediator _mediator;

    ///
    public AgentMsgConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Получить сообщение с инвентаризацией.
    /// </summary>
    /// <param name="message"> Сообщение с инвентаризацией. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(RigInventoryMsg message, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(new SaveRigInventoryCommand(message), cancellationToken);
    }

    /// <summary>
    /// Получить сообщение с результатом запуска майнинга на риге.
    /// </summary>
    /// <param name="message"> Сообщение. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(StartMiningCommandResult message, CancellationToken cancellationToken = default)
    {
        //return Task.CompletedTask;
        return message.IsSuccess
            ? _mediator.Send(new UseCases.RigState.Mining.StartMiningCommand(new RigId(message.RigId)), cancellationToken)
            : _mediator.Send(new TerminateStartMiningCommand(new RigId(message.RigId)), cancellationToken);
    }

    /// <summary>
    /// Получить сообщение с результатом остановки майнинга на риге.
    /// </summary>
    /// <param name="message"> Сообщение. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(StopMiningCommandResult message, CancellationToken cancellationToken = default)
    {
        return message.IsSuccess
            ? _mediator.Send(new UseCases.RigState.Mining.StopMiningCommand(new RigId(message.RigId)), cancellationToken)
            : _mediator.Send(new TerminateStopMiningCommand(new RigId(message.RigId)), cancellationToken);
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

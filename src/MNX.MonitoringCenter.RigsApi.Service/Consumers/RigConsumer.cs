using MediatR;
using EasyNetQ.AutoSubscribe;
using MNX.RigCommander.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.SecurityManagement.Authentication.Contracts;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.ConfirmFlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Service.Consumers;

/// <summary>
/// Потребитель сообщений от ригов.
/// </summary>
public class RigConsumer :
    IConsumeAsync<RigInventoryMsg>,
    IConsumeAsync<AgentRegisteredMsg>,
    IConsumeAsync<AgentConnectedMsg>,
    IConsumeAsync<AgentDisconnectedMsg>,
    IConsumeAsync<ApplyWorkerSettingsCommandResult>
{
    private readonly IMediator _mediator;

    public RigConsumer(IMediator mediator)
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
        return Task.WhenAll(

            _mediator.Send(new SaveRigInventoryCommand(message), cancellationToken),

            _mediator.Send(
                new SetRigDevicesCommand(message.RigId, message.RigOwnerId,
                                         message.Inventory.Gpus, message.Inventory.Cpus),
                cancellationToken)
        );
    }

    /// <summary>
    /// Получить сообщение о регистрации Агента.
    /// </summary>
    /// <param name="message"> Сообщение о регистрации Агента. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(AgentRegisteredMsg message, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(new AddRigCommand(message.Id, message.OwnerId, message.Nickname), cancellationToken);
    }

    /// <summary>
    /// Получить сообщение об установке соединения Агента с сервером.
    /// </summary>
    /// <param name="message"> Сообщение. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(AgentConnectedMsg message, CancellationToken cancellationToken = default)
    {
        return _mediator.Publish(new RigConnectedEvent(message.AgentId), cancellationToken);
    }

    /// <summary>
    /// Получить сообщение об отключении Агента от сервера.
    /// </summary>
    /// <param name="message"> Сообщение. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(AgentDisconnectedMsg message, CancellationToken cancellationToken = default)
    {
        return Task.WhenAll(
            _mediator.Publish(new Management.UseCases.RigDisconnectedEvent(message.AgentId), cancellationToken),
            _mediator.Publish(new Inventory.UseCases.RigDisconnectedEvent(message.AgentId), cancellationToken)
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


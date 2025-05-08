using MediatR;
using EasyNetQ.AutoSubscribe;
using MNX.RigCommander.Contracts;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.SecurityManagement.Authentication.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;

namespace MNX.MonitoringCenter.RigsApi.Service.Consumers;

/// <summary>
/// Потребитель сообщений об изменении жизненного цикла Агента.
/// </summary>
public class AgentLifeCycleMsgConsumer :
    IConsumeAsync<AgentRegisteredMsg>,
    IConsumeAsync<AgentConnectedMsg>,
    IConsumeAsync<AgentDisconnectedMsg>
{
    private readonly IMediator _mediator;

    public AgentLifeCycleMsgConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
}


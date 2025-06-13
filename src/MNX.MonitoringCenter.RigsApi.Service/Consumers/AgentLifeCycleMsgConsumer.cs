using EasyNetQ.AutoSubscribe;
using MediatR;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;
using MNX.MonitoringCenter.RigsApi.UseCases;
using MNX.MonitoringCenter.RigsApi.UseCases.RigState.Power;
using MNX.RigCommander.Contracts;
using MNX.SecurityManagement.Authentication.Contracts;

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

    ///
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
        var command = new AddRigCommand(new RigId(message.Id), message.OwnerId, message.Nickname);
        return _mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Получить сообщение об установке соединения Агента с сервером.
    /// </summary>
    /// <param name="message"> Сообщение. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(AgentConnectedMsg message, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(new TurnOnCommand(new RigId(message.AgentId)), cancellationToken);
    }

    /// <summary>
    /// Получить сообщение об отключении Агента от сервера.
    /// </summary>
    /// <param name="message"> Сообщение. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(AgentDisconnectedMsg message, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(new PowerOffCommand(new RigId(message.AgentId)), cancellationToken);
    }
}


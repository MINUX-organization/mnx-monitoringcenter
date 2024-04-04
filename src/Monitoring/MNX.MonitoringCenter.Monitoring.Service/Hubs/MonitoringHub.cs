using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.Infrastructure;
using MNX.MonitoringCenter.Monitoring.Service.Hubs.Clients;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.DisconnectClient;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.SubscribeClient;

namespace MNX.MonitoringCenter.Monitoring.Hubs;

/// <summary>
/// Хаб для взаимодействия с пользователями.
/// </summary>
[Authorize]
public class MonitoringHub : Hub<IMonitoringClient>
{
    /// <summary>
    /// Медиатор.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Сервис для доступа к данным пользователя.
    /// </summary>
    private readonly UserAccessor _userAccessor;

    public MonitoringHub(IMediator mediator, UserAccessor userProfile)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _userAccessor = userProfile ?? throw new ArgumentNullException(nameof(userProfile));
    }

    /// <summary>
    /// Подключение к сервису.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var rigs = _mediator.CreateStream(new SubscribeClientCommand(new ConnectionModel()
        {
            UserId = _userAccessor.GetUserId(),
            ConnectionId = Context.ConnectionId
        }));  

        await Clients.Client(Context.ConnectionId).ReceivedRigsInformation(rigs);
    }

    /// <summary>
    /// Отключение от сервиса.
    /// </summary>
    /// <param name="exception"> Возникшее исключение. </param>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _mediator.Send(new DisconnectClientCommand(new ConnectionModel()
        {
            UserId = _userAccessor.GetUserId(),
            ConnectionId = Context.ConnectionId
        }));
    }
}

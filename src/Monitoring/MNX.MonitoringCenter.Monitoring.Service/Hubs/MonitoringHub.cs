using MediatR;
using System.Security.Claims;
using MNX.MonitoringCenter.Monitoring.Service.Hubs.Clients;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.SubscribeClient;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.DisconnectClient;

namespace MNX.MonitoringCenter.Monitoring.Hubs;

/// <summary>
/// Хаб для взаимодействия с пользователями.
/// </summary>
public class MonitoringHub : Hub<IMonitoringClient>
{
    private readonly IMediator _mediator;

    public MonitoringHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Подключение к сервису.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var rigs = _mediator.CreateStream(new SubscribeClientCommand(new ConnectionModel()
        {
            UserId = GetUserId(),
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
            UserId = GetUserId(),
            ConnectionId = Context.ConnectionId
        }));
    }

    /// <summary>
    /// Получить идентификатор пользователя.
    /// </summary>
    /// <returns> Идентификатор пользователя. </returns>
    private long GetUserId()
    {
        return long.Parse(Context.GetHttpContext()?
                                 .User
                                 .Claims.First(x => x.Type == ClaimTypes.NameIdentifier.ToString()).Value!);
    }
}

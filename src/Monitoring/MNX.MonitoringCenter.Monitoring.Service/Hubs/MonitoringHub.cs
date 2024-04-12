using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.Infrastructure;
using MNX.MonitoringCenter.Monitoring.Service.Hubs.Clients;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.Hubs;

/// <summary>
/// Хаб для взаимодействия с пользователями.
/// </summary>
[Authorize]
public class MonitoringHub : Hub<IMonitoringClient>
{
    /// <summary>
    /// Наблюдатель за ригами.
    /// </summary>
    private readonly IUserRigsObserverWrapper _observer;

    /// <summary>
    /// Сервис для доступа к данным пользователя.
    /// </summary>
    private readonly UserAccessor _userAccessor;

    public MonitoringHub(IUserRigsObserverWrapper observer,
                         UserAccessor userProfile)
    {
        _observer = observer ?? throw new ArgumentNullException(nameof(observer));
        _userAccessor = userProfile ?? throw new ArgumentNullException(nameof(userProfile));
    }

    /// <summary>
    /// Подключение к сервису.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        await _observer.AddNewSubscriber(_userAccessor.GetUserId(), Context.ConnectionId);
    }

    /// <summary>
    /// Отключение от сервиса.
    /// </summary>
    /// <param name="exception"> Возникшее исключение. </param>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _observer.RemoveSubscriber(_userAccessor.GetUserId(), Context.ConnectionId);
    }

    /// <summary>
    /// Выбрать монету, по которой будет идти мониторинг скорости хеширования.
    /// </summary>
    /// <param name="coin"> Монета. </param>
    public async Task SendCoin(string coin)
    {
        await _observer.SetObservableCoin(coin, _userAccessor.GetUserId(), Context.ConnectionId);
    }
}

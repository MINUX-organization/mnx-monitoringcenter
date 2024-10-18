using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.RigsApi.Service.Infrastructure;
using MNX.MonitoringCenter.Traffic.Observers;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using System.Threading.Channels;

namespace MNX.MonitoringCenter.RigsApi.Service.Hubs;

/// <summary>
/// Хаб мониторинга.
/// </summary>
[Authorize]
public class MonitoringHub : Hub
{
    private readonly UserAccessor _userAccessor;

    private readonly IUserRigsObserverAggregator _userRigsObserverAggregator;

    public MonitoringHub(UserAccessor userAccessor,
                         IUserRigsObserverAggregator userRigsObserverAggregator)
    {
        _userAccessor = userAccessor
            ?? throw new ArgumentNullException(nameof(userAccessor));

        _userRigsObserverAggregator = userRigsObserverAggregator
            ?? throw new ArgumentNullException(nameof(userRigsObserverAggregator));
    }

    /// <summary>
    /// Отключение от сервиса.
    /// </summary>
    /// <param name="exception"> Возникшее исключение. </param>
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = _userAccessor.GetUserId();
        _userRigsObserverAggregator.UnsubscribeFromAll(userId, Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Подписаться на поток показателей.
    /// </summary>
    /// <param name="subscriptionType"> Тип подписки. </param>
    /// <returns> Читатель из канала. </returns>
    public ChannelReader<object> Subscribe(SubscriptionType subscriptionType)
    {
        var userId = _userAccessor.GetUserId();
        var channel = Channel.CreateUnbounded<object>();

        _userRigsObserverAggregator.TrySubscribe(userId, Context.ConnectionId,
                                                 subscriptionType, channel.Writer);

        return channel.Reader;
    }
}

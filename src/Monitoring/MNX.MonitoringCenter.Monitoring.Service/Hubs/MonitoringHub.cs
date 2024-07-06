using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.Infrastructure;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;
using MNX.MonitoringCenter.Monitoring.Service.Hubs.Clients;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.Devices.Gpu.SetOverclocking;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

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

    /// <summary>
    /// Посредник.
    /// </summary>
    private readonly IMediator _mediator;

    public MonitoringHub(IUserRigsObserverWrapper observer,
                         UserAccessor userProfile,
                         IMediator mediator)
    {
        _observer = observer ?? throw new ArgumentNullException(nameof(observer));
        _userAccessor = userProfile ?? throw new ArgumentNullException(nameof(userProfile));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Подключение к сервису.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        bool subscribeToDynamicDataStream = false;

        if (Context.GetHttpContext()!.Request.Query.TryGetValue("SubscribeToDynamicDataStream", out var argument))
        {
            if (bool.TryParse(argument, out var _))
            {
                subscribeToDynamicDataStream = true;
            }
        }

        await _observer.AddNewSubscriber(_userAccessor.GetUserId(),
                                         Context.ConnectionId,
                                         subscribeToDynamicDataStream);
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

    /// <summary>
    /// Задать строку поиска.
    /// </summary>
    /// <param name="searchString"> Строка поиска. </param>
    public Task SendSearchString(string searchString)
    {
        _observer.SetSearchString(searchString, _userAccessor.GetUserId(), Context.ConnectionId);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Задать разгон видеокарте.
    /// </summary>
    /// <param name="cardId"> Идентификатор видеокарты. </param>
    /// <param name="overclocking"> Разгон. </param>
    public async Task SetOverclocking(Guid cardId, GpuOverclockingModel overclocking)
    {
        var command = new SetGpuOverclockingCommand(
            _userAccessor.GetUserId(), Context.ConnectionId, cardId, overclocking);

        var result = await _mediator.Send(command);

        if (! result.IsSuccess)
        {
            await _mediator.Publish(
                new OverclockingSettingFailEvent(Context.ConnectionId, cardId, result.Errors?.ToArray()));
        }
    }
}

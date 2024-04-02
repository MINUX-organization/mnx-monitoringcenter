using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.Monitoring.Contracts.Messages;
using MNX.MonitoringCenter.Monitoring.Hubs;
using MNX.MonitoringCenter.Monitoring.Service.Hubs.Clients;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using MNX.MonitoringCenter.Monitoring.UseCases;

namespace MNX.MonitoringCenter.Monitoring.Service.Consumers;

/// <summary>
/// Потребитель сообщений от фермы.
/// </summary>
public class CommanderConsumer : IConsumeAsync<GotRigsStateMessage>, IConsumeAsync<GotRigsDynamicData>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CommanderConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary>
    /// Получить сообщение состояния ригов.
    /// </summary>
    /// <param name="message"> Сообщение состояния ригов. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns></returns>
    public async Task ConsumeAsync(GotRigsStateMessage message, CancellationToken cancellationToken = default)
    {
        var hubContext = GetHubContext();

        var rigRepository = GetRigRepository();

        message.RigsState = await Helper.MapDbDataToRigDataAsync
            (rigRepository.GetAvailable(message.UserId), message.RigsState);

        await hubContext.Clients.Client(message.ConnectionId).ReceivedRigsState(message.RigsState);
    }

    /// <summary>
    /// Получить сообщение динамических данных ригов.
    /// </summary>
    /// <param name="message"> Сообщение динамических данных ригов. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns></returns>
    public async Task ConsumeAsync(GotRigsDynamicData message, CancellationToken cancellationToken = default)
    {
        var hubContext = GetHubContext();

        var rigRepository = GetRigRepository();

        message.Data = await Helper.MapDbDataToRigDataAsync
            (rigRepository.GetAvailable(message.UserId), message.Data);

        await hubContext.Clients.User(message.UserId.ToString()).ReceivedRigsDynamicData(message.Data);
    }

    /// <summary>
    /// Получить контекст хаба.
    /// </summary>
    /// <returns> Контекст хаба. </returns>
    private IHubContext<MonitoringHub, IMonitoringClient> GetHubContext()
    {
        var scope = _serviceScopeFactory.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IHubContext<MonitoringHub, IMonitoringClient>>();
    }

    /// <summary>
    /// Получить репозиторий ригов.
    /// </summary>
    /// <returns> Репозиторий ригов. </returns>
    private IRigRepository GetRigRepository()
    {
        var scope = _serviceScopeFactory.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IRigRepository>();
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.RigsApi.UnionStreams.Args;
using MNX.MonitoringCenter.RigsApi.Service.Infrastructure;
using MNX.MonitoringCenter.RigsApi.UnionStreams.Abstractions;

namespace MNX.MonitoringCenter.RigsApi.Service.Hubs;

/// <summary>
/// Хаб мониторинга.
/// </summary>
[Authorize]
public class MonitoringHub : Hub
{
    private readonly UserAccessor _userAccessor;

    private readonly IUnionStreamBuilder _unionStreamBuilder;

    private readonly ILogger<MonitoringHub> _logger;

    public MonitoringHub(UserAccessor userAccessor,
                         IUnionStreamBuilder unionStreamBuilder,
                         ILogger<MonitoringHub> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _userAccessor = userAccessor
            ?? throw new ArgumentNullException(nameof(userAccessor));

        _unionStreamBuilder = unionStreamBuilder
            ?? throw new ArgumentNullException(nameof(unionStreamBuilder));
    }

    /// <summary>
    /// Отключение от сервиса.
    /// </summary>
    /// <param name="exception"> Возникшее исключение. </param>
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Unsubscribe();

        _logger.LogTrace("Disconnected: {ConnectionId}", Context.ConnectionId);

        return base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Подписаться на поток показателей.
    /// </summary>
    /// <param name="streamType"> Тип потока. </param>
    /// <returns> Поток данных. </returns>
    public async IAsyncEnumerable<object> Subscribe(StreamType streamType)
    {
        var userId = _userAccessor.GetUserId();

        _logger.LogTrace("Connected: {userId}, Type: {streamType}", userId, streamType);

        var stream = _unionStreamBuilder.Build(
            new UnionStreamBuilderArgs(userId, Context.ConnectionId, streamType));

        Context.Items.Add(userId, stream);

        await foreach (var response in stream.StartStreaming())
        {
            if (response is not null) 
                yield return response;
        }
    }

    /// <summary>
    /// Отписаться от потока показателей.
    /// </summary>
    public void Unsubscribe() 
    {
        var userId = _userAccessor.GetUserId();

        if (Context.Items.TryGetValue(userId, out var stream))
        {
            if (stream is UnionStreams.Abstractions.Stream s)
            {
                s.StopStreaming(userId, Context.ConnectionId);
            }
        }

        Context.Items.Remove(userId);

        _logger.LogTrace("Unsubscribed: {userId}", userId);
    }
}
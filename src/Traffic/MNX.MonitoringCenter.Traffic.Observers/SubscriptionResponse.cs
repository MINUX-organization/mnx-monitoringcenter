using System.Threading.Channels;

namespace MNX.MonitoringCenter.Traffic.Observers;

/// <summary>
/// Ответ на подписку.
/// </summary>
public class SubscriptionResponse
{
    /// <summary>
    /// Тип подписки.
    /// </summary>
    public required SubscriptionType Type { get; init; }

    /// <summary>
    /// Читатель канала.
    /// </summary>
    public required ChannelReader<object> Reader { get; init; }
}

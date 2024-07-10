using MediatR;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

/// <summary>
/// Событие об остановки потока динамических данных.
/// </summary>
public class DynamicDataStreamStoppingEvent : INotification
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; }

    public DynamicDataStreamStoppingEvent(long userId)
    {
        UserId = userId;
    }
}

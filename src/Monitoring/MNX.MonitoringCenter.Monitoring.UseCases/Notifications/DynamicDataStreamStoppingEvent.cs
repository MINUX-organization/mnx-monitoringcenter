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
    public Guid UserId { get; }

    public DynamicDataStreamStoppingEvent(Guid userId)
    {
        UserId = userId;
    }
}

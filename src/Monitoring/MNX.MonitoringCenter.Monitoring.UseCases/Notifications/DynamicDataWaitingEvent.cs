using MediatR;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

/// <summary>
/// Событие об ожидании динамических данных с ригов.
/// </summary>
public class DynamicDataWaitingEvent : INotification
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    public DynamicDataWaitingEvent(Guid userId)
    {
        UserId = userId;
    }
}

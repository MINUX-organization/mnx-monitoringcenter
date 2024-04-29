using MediatR;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications.UpdateTotalDynamicData;

/// <summary>
/// Событие об обновлении обобщённых динамических данных.
/// </summary>
public class UpdateTotalDynamicDataEvent : INotification
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; }

    /// <summary>
    /// Новые обобщённые данные.
    /// </summary>
    public TotalDynamicData Total { get; }

    public UpdateTotalDynamicDataEvent(long userId, TotalDynamicData total)
    {
        UserId = userId;
        Total = total;
    }
}

namespace MNX.MonitoringCenter.RigsApi.Service.Hubs.Notification;

/// <summary>
/// Интерфейс хаба оповещения фронта об
/// изменении состояния майнинг-устройства.
/// </summary>
public interface INotificationHubClient
{
    /// <summary>
    /// Отправка сигнала на фронт на метод
    /// MiningDeviceStateChanged.
    /// </summary>
    Task MiningDeviceStateChanged();
}

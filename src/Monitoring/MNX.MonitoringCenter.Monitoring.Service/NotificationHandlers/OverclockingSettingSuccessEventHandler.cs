using MediatR;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

public class OverclockingSettingSuccessEventHandler : INotificationHandler<OverclockingSettingSuccessEvent>
{
    /// <summary>
    /// Репозиторий девайсов для майнинг
    /// </summary>
    private readonly IMiningDeviceRepository _deviceRepository;

    public OverclockingSettingSuccessEventHandler(IMiningDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
    }

    public async Task Handle(OverclockingSettingSuccessEvent notification, CancellationToken cancellationToken)
    {
        await _deviceRepository.SetOverclockingById(notification.CardId, notification.Overclocking);
    }
}

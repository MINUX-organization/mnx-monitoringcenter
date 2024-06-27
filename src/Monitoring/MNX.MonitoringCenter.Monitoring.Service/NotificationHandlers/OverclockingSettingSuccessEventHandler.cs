using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

/// <summary>
/// Обработчик события об удачном применении разгона видеокарте.
/// </summary>
public class OverclockingSettingSuccessEventHandler : INotificationHandler<OverclockingSettingSuccessEvent>
{
    /// <summary>
    /// Репозиторий для разгонов видеокарт.
    /// </summary>
    private readonly IOverclockingRepository _overclockingRepository;

    public OverclockingSettingSuccessEventHandler(IOverclockingRepository overclockingRepository)
    {
        _overclockingRepository = overclockingRepository ?? throw new ArgumentNullException(nameof(overclockingRepository));
    }

    public async Task Handle(OverclockingSettingSuccessEvent request, CancellationToken cancellationToken)
    {
        await _overclockingRepository.SetOverclockingById(request.CardId, request.Overclocking);
    }
}

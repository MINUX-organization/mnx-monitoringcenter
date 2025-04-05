using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases;

/// <summary>
/// Событие на изменение состояния майнинг-устройств.
/// </summary>
/// <param name="ClientId"> Идентификатор клиента. </param>
public sealed record MiningDeviceStateChangedEvent(string ClientId)
    : INotification;
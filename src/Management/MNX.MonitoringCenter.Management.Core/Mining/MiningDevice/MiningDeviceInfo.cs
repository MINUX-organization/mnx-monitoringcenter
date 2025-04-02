using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;

/// <summary>
/// Информация о майнинга устройстве.
/// </summary>
public class MiningDeviceInfo : MiningDevice
{
    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    public Guid? RigId { get; set; }

    /// <summary>
    /// Признак нахождения устройства в сети.
    /// </summary>
    public bool IsOnline
    {
        get => _lifeCycleStatus == MiningDeviceLifeCycleStatus.Online;
    }

    /// <summary>
    /// Статус жизненного цикла.
    /// </summary>
    private MiningDeviceLifeCycleStatus _lifeCycleStatus;
    public MiningDeviceLifeCycleStatus LifeCycleStatus
    {
        get => _lifeCycleStatus;
        init => _lifeCycleStatus = value;
    }

    /// <summary>
    /// Идентификатор полётного листа.
    /// </summary>
    public Guid? FlightSheetId { get; set; }

    /// <summary>
    /// Признак подтверждения текущего значения полётного листа.
    /// </summary>
    /// <returns>
    /// <see cref="FlightSheetConfirmationState.Unconfirmed"/>, если  значение не подтверждено ригом,
    /// <see cref="FlightSheetConfirmationState.Successfully"/>, если значение подтверждено ригом,
    /// <see cref="FlightSheetConfirmationState.Error"/>, если произошла ошибка при подтверждении.
    /// </returns>
    public FlightSheetConfirmationState FlightSheetConfirmationState { get; set; }
        = FlightSheetConfirmationState.Unconfirmed;

    /// <summary>
    /// Полётный лист.
    /// </summary>
    public FlightSheet.FlightSheet? FlightSheet { get; set; }

    /// <summary>
    /// Идентификатор пресета.
    /// </summary>
    public Guid? PresetId { get; set; }

    /// <summary>
    /// Пресет.
    /// </summary>
    public Preset? Preset { get; set; }

    /// <summary>
    /// Деактивировать.
    /// </summary>
    public void Deactivate()
    {
        if (LifeCycleStatus == MiningDeviceLifeCycleStatus.Online)
            throw new ApplicationException("It is not possible to switch from the \"online\" state to the \"inactive\" state.");

        _lifeCycleStatus = MiningDeviceLifeCycleStatus.Inactive;
        RigId = null;
        OwnerId = null;
    }

    /// <summary>
    /// Перевести в состояние "в сети".
    /// </summary>
    public void SwitchToOnline()
    {
        _lifeCycleStatus = MiningDeviceLifeCycleStatus.Online;
    }

    /// <summary>
    /// Перевести в состояние "не в сети".
    /// </summary>
    public void SwitchToOffline()
    {
        if (LifeCycleStatus == MiningDeviceLifeCycleStatus.Inactive)
            throw new ApplicationException("It is not possible to switch from the \"inactive\" state to the \"offline\" state.");

        _lifeCycleStatus = MiningDeviceLifeCycleStatus.Offline;
    }
}

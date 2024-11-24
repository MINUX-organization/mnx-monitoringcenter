using MNX.MonitoringCenter.Management.Core.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Core.MiningDevice;

/// <summary>
/// Информация о майнинга устройстве.
/// </summary>
public class MiningDeviceInfo : MiningDevice
{
    /// <summary>
    /// Признак активности устройства ( в данных момент установлен на риге ).
    /// </summary>
    public bool IsActive
    {
        get => LifeCycleStatus != MiningDeviceLifeCycleStatus.Inactive;
    }

    /// <summary>
    /// Статус жизненного цикла.
    /// </summary>
    private MiningDeviceLifeCycleStatus _lifeCycleStatus;
    public MiningDeviceLifeCycleStatus LifeCycleStatus
    {
        get => _lifeCycleStatus;
        set
        {
            // todo: реализовать машину состояний

            if (_lifeCycleStatus == MiningDeviceLifeCycleStatus.Online)
            {
                if (value == MiningDeviceLifeCycleStatus.Inactive)
                    throw new ApplicationException("It is not possible to switch from the \"online\" state to the \"inactive\" state.");

                _lifeCycleStatus = value;
                return;
            }

            if (_lifeCycleStatus == MiningDeviceLifeCycleStatus.Inactive)
            {
                if (value == MiningDeviceLifeCycleStatus.Offline)
                    throw new ApplicationException("It is not possible to switch from the \"inactive\" state to the \"offline\" state.");

                _lifeCycleStatus = value;
                return;
            }

            _lifeCycleStatus = value;
        }
    }

    /// <summary>
    /// Идентификатор полётного листа.
    /// </summary>
    public Guid? FlightSheetId { get; set; }

    /// <summary>
    /// Признак подтверждения текущего значения полётного листа.
    /// </summary>
    /// <returns>
    /// <see langword="true"/>, если  значение подтверждено ригом, иначе <see langword="false"/>.
    /// </returns>
    public bool FlightSheetIsConfirm { get; set; } = true;

    /// <summary>
    /// Полётный лист.
    /// </summary>
    public FlightSheet.FlightSheet? FlightSheet { get; set; }
}

namespace MNX.MonitoringCenter.Management.Core.MiningDevice;

/// <summary>
/// Информация о майнинга устройстве.
/// </summary>
public class MiningDeviceInfo : MiningDevice
{
    /// <summary>
    /// Признак активности устройства ( в данных момент установлен на риге ).
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Идентификатор полётного листа.
    /// </summary>
    public Guid? FlightSheetId { get; set; }

    /// <summary>
    /// Полётный лист.
    /// </summary>
    public FlightSheet.FlightSheet? FlightSheet { get; set; }
}

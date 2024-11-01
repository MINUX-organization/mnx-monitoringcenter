namespace MNX.MonitoringCenter.Management.Core.MiningDevice;

/// <summary>
/// Майнинг устройство c деталями.
/// </summary>
public class MiningDeviceDetails : MiningDevice
{
    /// <summary>
    /// Признак активности устройства ( в данных момент установлен на риге ).
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Идентификатор полётного листа.
    /// </summary>
    public Guid? FLightSheetId { get; set; }
}

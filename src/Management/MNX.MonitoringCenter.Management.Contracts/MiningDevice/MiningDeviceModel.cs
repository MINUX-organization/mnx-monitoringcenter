namespace MNX.MonitoringCenter.Management.Contracts.MiningDevice;

/// <summary>
/// Модуль майнинга устройства.
/// </summary>
public class MiningDeviceModel
{
    /// <summary>
    /// Идентификатор майнинг устройства.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    public Guid RigId { get; init; }

    /// <summary>
    /// Тип майнинг устройства.
    /// </summary>
    //public MiningDeviceType Type { get; init; }

    /// <summary>
    /// Название полётного листа.
    /// </summary>
    public string? FlightSheetName { get; init; }
    
    /// <summary>
    /// Название майнера.
    /// </summary>
    public string? MinerName { get; init; }
}

using MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Dto.Devices;

/// <summary>
/// Майнинг устройство.
/// </summary>
public abstract class MiningDeviceDto
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    public Guid RigId { get; set; }

    /// <summary>
    /// Риг.
    /// </summary>
    public RigDto? Rig { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Тип.
    /// </summary>
    public MiningDeviceType Type { get; set; }

    /// <summary>
    /// Производитель.
    /// </summary>
    public string Manufacturer { get; set; }

    /// <summary>
    /// Идентификатор полётного листа.
    /// </summary>
    public Guid? FlightSheetId { get; set; }

    /// <summary>
    /// Полётный лист.
    /// </summary>
    public FlightSheetDto? FlightSheet { get; set; }

    /// <summary>
    /// Идентификатор разгона.
    /// </summary>
    public Guid? OverclockingId { get; set; }

    /// <summary>
    /// Разгон.
    /// </summary>
    public OverclockingDto? Overclocking { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public string? Miner { get; set; }

    /// <summary>
    /// Серийный номер.
    /// </summary>
    public string SerialNumber { get; set; }
}

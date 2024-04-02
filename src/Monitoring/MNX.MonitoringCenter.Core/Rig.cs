namespace MNX.MonitoringCenter.Monitoring.Core;

/// <summary>
/// Риг.
/// </summary>
public class Rig
{
    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Локальный IP-адрес.
    /// </summary>
    public string LocalIP { get; set; }

    /// <summary>
    /// Версия Minux.
    /// </summary>
    public string MinuxVersion { get; set; }

    /// <summary>
    /// Количество карт Nvidia.
    /// </summary>
    public int NvidiaCount { get; set; }

    /// <summary>
    /// Количество карт Amd.
    /// </summary>
    public int AmdCount { get; set; }

    /// <summary>
    /// Количество карт Intel.
    /// </summary>
    public int IntelCount { get; set; }

    /// <summary>
    /// Информация о полётных листах.
    /// </summary>
    public List<FlightSheet> FlightSheetInfo { get; set; } = new();
}

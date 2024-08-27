namespace MNX.MonitoringCenter.Inventory.Contracts;

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
    /// Глобальный IP-адрес.
    /// </summary>
    public string GlobalIP { get; set; }

    /// <summary>
    /// Локальный IP-адрес.
    /// </summary>
    public string LocalIP { get; set; }

    /// <summary>
    /// MAC адрес.
    /// </summary>
    public string Mac { get; set; }
}

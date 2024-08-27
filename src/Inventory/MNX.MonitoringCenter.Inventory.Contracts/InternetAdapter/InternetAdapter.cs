namespace MNX.MonitoringCenter.Inventory.Contracts.InternetAdapter;

/// <summary>
/// Интернет адаптер.
/// </summary>
public class InternetAdapter
{
    /// <summary>
    /// Идентификатор.
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
    /// Информация.
    /// </summary>
    public InternetAdapterInformation Information { get; set; }
}

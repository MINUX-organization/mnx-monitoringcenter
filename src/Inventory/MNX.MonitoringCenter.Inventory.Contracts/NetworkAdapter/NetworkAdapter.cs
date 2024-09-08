namespace MNX.MonitoringCenter.Inventory.Contracts.NetworkAdapter;

/// <summary>
/// Сетевой адаптер.
/// </summary>
public record NetworkAdapter
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Глобальный IP-адрес.
    /// </summary>
    public string? GlobalIP { get; init; }

    /// <summary>
    /// Локальный IP-адрес.
    /// </summary>
    public string? LocalIP { get; init; }

    /// <summary>
    /// Информация.
    /// </summary>
    public required NetworkAdapterInformation Information { get; init; }

    /// <summary>
    /// Получить признак активности адаптера ( подключен к сети Интернет ).
    /// </summary>
    /// <returns>
    /// <see langword="true"/>, если активен, иначе <see langword="false"/>.
    /// </returns>
    public bool IsActive() => GlobalIP != null;
}

namespace MNX.MonitoringCenter.Inventory.Contracts.Motherboard;

/// <summary>
/// Материнская плата.
/// </summary>
public record Motherboard
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Информация о материнской плате.
    /// </summary>
    public required MotherboardInformation Information { get; init; }

    /// <summary>
    /// Список PCI.     
    /// </summary>
    public List<MotherboardPci> Pcies { get; init; } = new();
}

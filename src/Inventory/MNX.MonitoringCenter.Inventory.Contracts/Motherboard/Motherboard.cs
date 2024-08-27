namespace MNX.MonitoringCenter.Inventory.Contracts.Motherboard;

/// <summary>
/// Материнская плата.
/// </summary>
public class Motherboard
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Информация о материнской плате.
    /// </summary>
    public MotherboardInformation Information { get; set; }

    /// <summary>
    /// Список PCI.     
    /// </summary>
    public List<MotherboardPci> Pcies { get; set; }
}

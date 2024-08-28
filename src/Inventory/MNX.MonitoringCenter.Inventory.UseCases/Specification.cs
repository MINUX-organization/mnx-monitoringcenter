namespace MNX.MonitoringCenter.Inventory.UseCases;

/// <summary>
/// Спецификация инвентаризации.
/// </summary>
public readonly struct InventorySpecification
{
    /// <summary>
    /// Идентификаторы ригов.
    /// </summary>
    public Guid[]? RigsIds { get; }

    public InventorySpecification(Guid rigId)
    {
        RigsIds = new[] { rigId };
    }

    public InventorySpecification(Guid[]? rigsIds)
    {
        RigsIds = rigsIds;
    }
}

/// <summary>
/// Спецификация устройств.
/// </summary>
public readonly struct DeviceSpecification
{
    /// <summary>
    /// Спецификация инвентаризации.
    /// </summary>
    public InventorySpecification InventorySpecification { get; }

    /// <summary>
    /// Список моделей.
    /// </summary>
    public string[]? Models { get; }

    /// <summary>
    /// Производители.
    /// </summary>
    public string[]? Manufacturers { get; }

    public DeviceSpecification(Guid[] rigsIds,
                               string[]? models = null,
                               string[]? manufacturers = null)
    {
        InventorySpecification = new(rigsIds);
        Models = models;
        Manufacturers = manufacturers;
    }
}

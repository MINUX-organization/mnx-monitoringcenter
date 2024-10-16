namespace MNX.MonitoringCenter.Inventory.Contracts.Requests;

/// <summary>
/// Спецификация инвентаризации.
/// </summary>
public readonly struct InventorySpecification
{
    /// <summary>
    /// Идентификатор владельца.
    /// </summary>
    public Guid? OwnerId { get; }

    /// <summary>
    /// Идентификаторы ригов.
    /// </summary>
    public Guid[]? RigsIds { get; }

    /// <summary>
    /// Признак актуальности инвентаризации.
    /// </summary>
    public bool IsActuality { get; }

    public InventorySpecification(Guid? ownerId, Guid rigId, bool isActuality = false)
    {
        OwnerId = ownerId;
        RigsIds = new[] { rigId };
        IsActuality = isActuality;
    }

    public InventorySpecification(Guid? ownerId, Guid[]? rigsIds = null, bool isActuality = false)
    {
        OwnerId = ownerId;
        RigsIds = rigsIds;
        IsActuality = isActuality;
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

    /// <summary>
    /// Создаёт экземпляр структуры <see cref="DeviceSpecification"/>.
    /// </summary>
    /// <param name="ownerId"> Идентификатор владельца. </param>
    /// <param name="rigsIds"> Идентификаторы запрашиваемых ригов. Если нет, то все доступные риги. </param>
    /// <param name="models"> Запрашиваемые модели процессоров. Если нет, то все доступные модели. </param>
    /// <param name="manufacturers"> Запрашиваемый производители процессоров. Если нет, то все доступные производители. </param>
    public DeviceSpecification(Guid ownerId,
                               Guid[]? rigsIds = null,
                               string[]? models = null,
                               string[]? manufacturers = null)
    {
        InventorySpecification = new(ownerId, rigsIds, true);
        Models = models;
        Manufacturers = manufacturers;
    }

    public DeviceSpecification(Guid ownerId,
                               Guid rigId,
                               string[]? models = null,
                               string[]? manufacturers = null)
        : this(ownerId, new Guid[] { rigId }, models, manufacturers)
    { }
}

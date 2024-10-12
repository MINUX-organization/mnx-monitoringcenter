namespace MNX.MonitoringCenter.Inventory.Contracts.Queries;

// <summary>
/// Спецификация инвентаризации.
/// </summary>
public readonly struct InventorySpecification
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Идентификаторы ригов.
    /// </summary>
    public Guid[]? RigsIds { get; }

    public InventorySpecification(Guid userId, Guid rigId)
    {
        UserId = userId;
        RigsIds = new[] { rigId };
    }

    public InventorySpecification(Guid userId, Guid[]? rigsIds = null)
    {
        UserId = userId;
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

    /// <summary>
    /// Создаёт экземпляр структуры <see cref="DeviceSpecification"/>.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="rigsIds"> Идентификаторы запрашиваемых ригов. Если нет, то все доступные риги. </param>
    /// <param name="models"> Запрашиваемые модели процессоров. Если нет, то все доступные модели. </param>
    /// <param name="manufacturers"> Запрашиваемый производители процессоров. Если нет, то все доступные производители. </param>
    public DeviceSpecification(Guid userId,
                               Guid[]? rigsIds = null,
                               string[]? models = null,
                               string[]? manufacturers = null)
    {
        InventorySpecification = new(userId, rigsIds);
        Models = models;
        Manufacturers = manufacturers;
    }
}

using MNX.MonitoringCenter.Inventory.UseCases;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Расширения для <see cref="IQueryable{Inventory}"/>.
/// </summary>
internal static class InventoryQueryableExtensions
{
    /// <summary>
    /// Получить список доступных инвентаризаций.
    /// </summary>
    /// <param name="inventory"> Запрашиваемый список инвентаризаций. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Запрашиваемый список инвентаризаций. </returns>
    internal static IQueryable<Inventory> Available(this IQueryable<Inventory> inventory,
                                                    InventorySpecification specification)
    {
        return inventory.Where(x => x.RigOwnerId == specification.UserId);
    }

    /// <summary>
    /// Получить текущую инвентаризацию.
    /// </summary>
    /// <param name="inventory"> Запрашиваемый список записей инвентаризации. </param>
    /// <returns> Запрашиваемый список записей инвентаризации. </returns>
    internal static IQueryable<Inventory> GetCurrentInventory(this IQueryable<Inventory> inventory)
    {
        return inventory.Where(x => x.EndDateTime == null);
    }

    /// <summary>
    /// Отфильтровать.
    /// </summary>
    /// <param name="inventory"> Инвентаризация. </param>
    /// <param name="specification"> Спецификация </param>
    /// <returns> Инвентаризация. </returns>
    internal static IQueryable<Inventory> InventoryFilter(this IQueryable<Inventory> inventory,
                                                          InventorySpecification specification)
    {
        if (specification.RigsIds != null)
        {
            inventory = inventory.Where(x => specification.RigsIds.Contains(x.RigId));
        }

        return inventory;
    }

    /// <summary>
    /// Получить инвентаризацию за период.
    /// </summary>
    /// <param name="inventory"> Инвентаризация. </param>
    /// <param name="start"> Начало периода. </param>
    /// <param name="end"> Конец периода. </param>
    /// <returns> Инвентаризация. </returns>
    internal static IQueryable<Inventory> GetForAPeriod(this IQueryable<Inventory> inventory,
                                                        DateTimeOffset start,
                                                        DateTimeOffset end)
    {
        return inventory.Where(x => x.CreatedDateTime <= end && x.EndDateTime >= start);
    }
}

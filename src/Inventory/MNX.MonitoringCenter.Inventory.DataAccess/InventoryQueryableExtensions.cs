using MNX.MonitoringCenter.Inventory.Contracts.Requests;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Расширения для <see cref="IQueryable{Inventory}"/>.
/// </summary>
internal static class InventoryQueryableExtensions
{
    /// <summary>
    /// Получить актуальные инвентаризации.
    /// </summary>
    /// <param name="inventory"> Запрашиваемый список записей инвентаризации. </param>
    /// <returns> Запрашиваемый список записей инвентаризации. </returns>
    internal static IQueryable<Rigs.RigInventory> Actualize(this IQueryable<Rigs.RigInventory> inventory,
                                                                    InventorySpecification specification)
    {
        if (specification.IsActuality)
        {
            return inventory.Where(x => x.IsCurrent);
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
    internal static IQueryable<Rigs.RigInventory> GetForAPeriod(this IQueryable<Rigs.RigInventory> inventory,
                                                                        DateTimeOffset start,
                                                                        DateTimeOffset end)
    {
        return inventory.Where(x => x.CreatedDateTime <= end && x.EndDateTime >= start);
    }
}

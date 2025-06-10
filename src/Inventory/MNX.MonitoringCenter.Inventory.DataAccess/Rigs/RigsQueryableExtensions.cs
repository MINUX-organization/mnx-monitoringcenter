using MNX.MonitoringCenter.Inventory.Contracts.Requests;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs;

/// <summary>
/// Расширения для <see cref="IQueryable{RigDto}"/>
/// </summary>
internal static class RigsQueryableExtensions
{
    /// <summary>
    /// Получить список доступных ригов.
    /// </summary>
    /// <param name="rigs"> Запрашиваемый список ригов. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Запрашиваемый список ригов. </returns>
    internal static IQueryable<RigDto> Available(this IQueryable<RigDto> rigs,
                                                 InventorySpecification specification)
    {
        if (specification.OwnerId != null)
        {
            rigs = rigs.Where(x => x.OwnerId == specification.OwnerId);
        }

        return rigs;
    }

    /// <summary>
    /// Отфильтровать.
    /// </summary>
    /// <param name="rigs"> Запрашиваемый список ригов. </param>
    /// <param name="specification"> Спецификация </param>
    /// <returns> Запрашиваемый список ригов. </returns>
    internal static IQueryable<RigDto> Filter(this IQueryable<RigDto> rigs,
                                              InventorySpecification specification)
    {
        if (specification.RigsIds != null)
        {
            rigs = rigs.Where(x => specification.RigsIds.Contains(x.Id));
        }

        return rigs;
    }

    /// <summary>
    /// Сортировать сущности по алфавиту.
    /// </summary>
    /// <param name="entities"> Сущности. </param>
    /// <returns> Сортированные сущности. </returns>
    internal static IQueryable<RigDto> Sort(
        this IQueryable<RigDto> entities)
    {
        return entities.OrderBy(x => x.Name);
    }
}

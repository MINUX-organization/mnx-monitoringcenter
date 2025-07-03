using System.Linq.Dynamic.Core;
using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess.Pool;

using Pool = Core.Mining.Pool;

/// <summary>
/// Расширения функций <see cref="PoolRepository"/>.
/// </summary>
internal static class PoolQueryExtensions
{
    /// <summary>
    /// Получить доступные пользователю сущности.
    /// </summary>
    /// <param name="entities"> Сущности. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Доступные сущности. </returns>
    internal static IQueryable<Pool> Available(
        this IQueryable<Pool> entities, Specification specification)
    {
        return entities.Where(x => x.OwnerId == specification.UserId || x.OwnerId == null);
    }

    /// <summary>
    /// Получить доступные пользователю сущности.
    /// </summary>
    /// <param name="entities"> Сущности. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Доступные сущности. </returns>
    internal static IQueryable<Pool> Available(
        this IQueryable<Pool> entities, Guid userId)
    {
        return entities.Where(x => x.OwnerId == userId || x.OwnerId == null);
    }

    /// <summary>
    /// Сортировать сущности tls и идентификатору пользователя.
    /// </summary>
    /// <param name="entities"> Сущности. </param>
    /// <returns> Сортированные сущности. </returns>
    internal static IQueryable<Pool> Sort(
        this IQueryable<Pool> entities)
    {
        return entities.OrderBy(x => x.OwnerId == null)
                       .ThenBy(x => x.OwnerId)
                       .ThenBy(x => x.Tls == false);
    }
}

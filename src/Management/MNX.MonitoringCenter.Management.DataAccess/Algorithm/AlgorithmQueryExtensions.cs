using MNX.MonitoringCenter.Management.UseCases;
using System.Linq.Dynamic.Core;

namespace MNX.MonitoringCenter.Management.DataAccess.Algorithm;

using Algorithm = Core.Mining.Algorithm;

/// <summary>
/// Расширения функций <see cref="AlgorithmRepository"/>.
/// </summary>
internal static class AlgorithmQueryExtensions
{
    /// <summary>
    /// Получить доступные пользователю сущности.
    /// </summary>
    /// <param name="entities"> Сущности. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Доступные сущности. </returns>
    internal static IQueryable<Algorithm> Available(
        this IQueryable<Algorithm> entities, Specification specification)
    {
        return entities.Where(x => x.OwnerId == specification.UserId || x.OwnerId == null);
    }

    /// <summary>
    /// Получить доступные пользователю сущности.
    /// </summary>
    /// <param name="entities"> Сущности. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Доступные сущности. </returns>
    internal static IQueryable<Algorithm> Available(
        this IQueryable<Algorithm> entities, Guid userId)
    {
        return entities.Where(x => x.OwnerId == userId || x.OwnerId == null);
    }

    /// <summary>
    /// Сортировать сущности по алфавиту и идентификатору пользователя.
    /// </summary>
    /// <param name="entities"> Сущности. </param>
    /// <returns> Сортированные сущности. </returns>
    internal static IQueryable<Algorithm> Sort(
        this IQueryable<Algorithm> entities)
    {
        return entities.OrderBy(x => x.OwnerId == null)
                         .ThenBy(x => x.OwnerId)
                         .ThenBy(x => x.Name);
    }
}
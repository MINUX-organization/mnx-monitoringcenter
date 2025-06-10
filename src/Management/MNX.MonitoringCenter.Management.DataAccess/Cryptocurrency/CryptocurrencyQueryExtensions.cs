using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess.Cryptocurrency;

using Cryptocurrency = Core.Mining.Cryptocurrency;

/// <summary>
/// Расширения функицй <see cref="CryptocurrencyRepository"/>.
/// </summary>
internal static class CryptocurrencyQueryExtensions
{
    /// <summary>
    /// Получить доступные пользователю сущности.
    /// </summary>
    /// <param name="entities"> Сущности. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Доступные сущности. </returns>
    internal static IQueryable<Cryptocurrency> Available(
        this IQueryable<Cryptocurrency> entities, Specification specification)
    {
        return entities.Where(x => x.UserId == specification.UserId || x.UserId == null);
    }

    /// <summary>
    /// Получить доступные пользователю сущности.
    /// </summary>
    /// <param name="entities"> Сущности. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Доступные сущности. </returns>
    internal static IQueryable<Cryptocurrency> Available(
        this IQueryable<Cryptocurrency> entities, Guid userId)
    {
        return entities.Where(x => x.UserId == userId || x.UserId == null);
    }

    /// <summary>
    /// Сортировать сущности по алфавиту и идентификатору пользователя.
    /// </summary>
    /// <param name="entities"> Сущности. </param>
    /// <returns> Сортированные сущности. </returns>
    internal static IQueryable<Cryptocurrency> Sort(
        this IQueryable<Cryptocurrency> entities)
    {
        return entities.OrderBy(x => x.UserId == null)
                             .ThenBy(x => x.UserId)
                             .ThenBy(x => x.FullName);
    }
}

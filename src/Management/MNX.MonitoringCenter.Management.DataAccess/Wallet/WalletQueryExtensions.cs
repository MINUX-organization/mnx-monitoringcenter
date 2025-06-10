namespace MNX.MonitoringCenter.Management.DataAccess.Wallet;

using Wallet = Core.Mining.Wallet;

/// <summary>
/// Расширение функций <see cref="WalletRepository"/>.
/// </summary>
internal static class WalletQueryExtensions
{
    /// <summary>
    /// Сортировать сущности по алфавиту.
    /// </summary>
    /// <param name="entities"> Сущности. </param>
    /// <returns> Сортированные сущности. </returns>
    internal static IQueryable<Wallet> Sort(
        this IQueryable<Wallet> entities)
    {
        return entities.OrderBy(x => x.Name);
    }
}

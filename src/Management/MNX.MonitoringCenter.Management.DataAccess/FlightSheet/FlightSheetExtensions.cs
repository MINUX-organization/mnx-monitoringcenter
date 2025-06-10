namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

/// <summary>
/// Расширения функций <see cref="FlightSheetRepository"/>.
/// </summary>
internal static class FlightSheetExtensions
{
    /// <summary>
    /// Сортировать сущности по алфавиту.
    /// </summary>
    /// <param name="entities"> Сущности. </param>
    /// <returns> Сортированные сущности. </returns>
    internal static IQueryable<FlightSheet> Sort(
        this IQueryable<FlightSheet> entities)
    {
        return entities.OrderBy(x => x.Name);
    }
}

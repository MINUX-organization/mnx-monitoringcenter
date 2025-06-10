namespace MNX.MonitoringCenter.Management.DataAccess.Preset;

using Preset = Core.Overclocking.Preset;

/// <summary>
/// Расширения функций <see cref="PresetRepository"/>.
/// </summary>
internal static class PresetQueryExtensions
{
    /// <summary>
    /// Сортировать сущности по алфавиту.
    /// </summary>
    /// <param name="entities"> Сущности. </param>
    /// <returns> Сортированные сущности. </returns>
    internal static IQueryable<Preset> Sort(
        this IQueryable<Preset> entities)
    {
        return entities.OrderBy(x => x.Name);
    }
}

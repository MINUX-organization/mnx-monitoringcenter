using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts;

/// <summary>
/// Dto инвентаризации майнеров.
/// </summary>
[Table("miners_inventory")]
public class MinerInventory
{
    /// <summary>
    /// Идентификатор майнера.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Наименование майнера.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Версия майнера.
    /// </summary>
    public required string Version { get; init; }
}

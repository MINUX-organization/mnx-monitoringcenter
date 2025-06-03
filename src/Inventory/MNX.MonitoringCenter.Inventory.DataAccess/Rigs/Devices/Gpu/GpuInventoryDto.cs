namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu;

using Gpu = Contracts.Devices.Gpu.Gpu;

/// <summary>
/// Сущность видеокарты для базы данных.
/// </summary>
public record GpuInventoryDto : Gpu
{
    /// <summary>
    /// Идентификатор инвентаризации рига.
    /// </summary>
    public long RigInventoryId { get; init; }
}

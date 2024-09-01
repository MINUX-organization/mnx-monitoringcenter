using MNX.MonitoringCenter.Inventory.Contracts;

namespace MNX.MonitoringCenter.Inventory.UseCases.Software;

/// <summary>
/// Репозиторий для доступа к инвентаризации ПО.
/// </summary>
public interface ISoftwareRepository
{
    /// <summary>
    /// Получить текущую инвентаризацию программного обеспечения.
    /// </summary>
    /// <param name="specification"> Спецификация инвентаризации. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Текущая инвентаризация программного обеспечения </returns>
    Task<SoftwareInventory?> GetByRigId(InventorySpecification specification, CancellationToken cancellationToken);
}

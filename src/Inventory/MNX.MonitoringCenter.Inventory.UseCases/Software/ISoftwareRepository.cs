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
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Текущая инвентаризация программного обеспечения </returns>
    Task<SoftwareInventory?> GetByRigId(Guid rigId, CancellationToken cancellationToken);
}

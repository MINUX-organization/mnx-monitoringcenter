using MNX.MonitoringCenter.Inventory.Contracts;

namespace MNX.MonitoringCenter.Inventory.UseCases;

/// <summary>
/// Репозиторий для доступа к инвентаризации.
/// </summary>
public interface IInventoryRepository
{
    /// <summary>
    /// Сохранить инвентаризацию.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="createdDate"> Дата проведения инвентаризации. </param>
    /// <param name="inventory"> Результат инвентаризации. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task Save(Guid rigId, DateTimeOffset createdDate, InventoryModel inventory, CancellationToken cancellationToken);
}

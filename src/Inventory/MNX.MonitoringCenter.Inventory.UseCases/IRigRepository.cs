using MNX.MonitoringCenter.Inventory.Contracts.RigInventory;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts;

namespace MNX.MonitoringCenter.Inventory.UseCases;

/// <summary>
/// Репозиторий для доступа к ригам.
/// </summary>
public interface IRigRepository
{
    /// <summary>
    /// Получить список ригов.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Асинхронный поток ригов. </returns>
    IAsyncEnumerable<Rig> GetRigs(InventorySpecification specification);

    /// <summary>
    /// Добавить риг.
    /// </summary>
    /// <param name="rig"> Риг. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task Add(Rig rig, CancellationToken cancellationToken);

    /// <summary>
    /// Сохранить инвентаризацию рига.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="createdDate"> Дата проведения инвентаризации. </param>
    /// <param name="inventory"> Результат инвентаризации. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task SaveInventory(Guid rigId, DateTimeOffset createdDate,
                       RigInventoryModel inventory, CancellationToken cancellationToken);
}

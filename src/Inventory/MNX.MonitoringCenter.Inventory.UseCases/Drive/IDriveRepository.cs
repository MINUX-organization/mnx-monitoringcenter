using MNX.MonitoringCenter.Inventory.Contracts.Queries;

namespace MNX.MonitoringCenter.Inventory.UseCases.Drive;

/// <summary>
/// Репозиторий для доступа к дискам.
/// </summary>
public interface IDriveRepository
{
    /// <summary>
    /// Получить список дисков, подключенных к ригу.
    /// </summary>
    /// <param name="specification"> Спецификация инвентаризации. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Список дисков. </returns>
    Task<List<Contracts.Drive.Drive>?> GetList(InventorySpecification specification,
                                               CancellationToken cancellationToken);

    /// <summary>
    /// Получить кол-во дисков по спецификации.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Кол-во дисков, соответствующих спецификации. </returns>
    Task<int> GetCount(DeviceSpecification specification, CancellationToken cancellationToken);
}

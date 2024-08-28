namespace MNX.MonitoringCenter.Inventory.UseCases.Drive;

/// <summary>
/// Репозиторий для доступа к дискам.
/// </summary>
public interface IDriveRepository
{
    /// <summary>
    /// Получить список дисков, подключенных к ригу.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Список дисков. </returns>
    Task<List<Contracts.Drive.Drive>?> GetList(Guid rigId, CancellationToken cancellationToken);
}

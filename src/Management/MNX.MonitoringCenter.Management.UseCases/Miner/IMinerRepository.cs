namespace MNX.MonitoringCenter.Management.UseCases.Miner;

using Miner = Core.Miner.Miner;

/// <summary>
/// Репозиторий для доступа к майнерам
/// </summary>
public interface IMinerRepository
{
    /// <summary>
    /// Получить список доступных майнеров.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Список доступных майнеров. </returns>
    IAsyncEnumerable<Miner> GetAvailableMiners(Specification specification);

    /// <summary>
    /// Получить майнер по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор майнера. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Майнер. </returns>
    Task<Miner?> GetMinerById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получить майнер по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Признак существования майнера. </returns>
    Task<bool> Exists(Guid id, CancellationToken cancellationToken);
}
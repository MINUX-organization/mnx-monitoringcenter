
namespace MNX.MonitoringCenter.Management.UseCases.Miner;

/// <summary>
/// Репозиторий для доступа к майнерам
/// </summary>
public interface IMinerRepository
{
    /// <summary>
    /// Получить список доступных майнеров.
    /// </summary>
    /// <returns> Список доступных майнеров. </returns>
    IAsyncEnumerable<Core.Miner> GetAvailableMiners();

    /// <summary>
    /// Получить признак существования майнера.
    /// </summary>
    /// <param name="name"> Название майнера. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns>
    /// <see langword="true"/>, если майнер существует, иначе <see langword="false"/>.
    /// </returns>
    Task<bool> Exists(string name, CancellationToken cancellationToken = default);
}
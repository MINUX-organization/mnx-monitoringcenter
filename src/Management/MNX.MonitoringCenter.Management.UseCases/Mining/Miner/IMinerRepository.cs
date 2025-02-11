using MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;
using MNX.MonitoringCenter.Management.Core.Mining.Miner;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

using Miner = Core.Mining.Miner.Miner;

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

    /// <summary>
    /// Получить все привязки относительных наименований алгоритма к майнерам.
    /// </summary>
    /// <param name="algorithmId"> Идентификатор алгоритма. </param>
    /// <returns> Список относительных наименований алгоритма для манеров. </returns>
    IAsyncEnumerable<MinerAlgorithm> GetMinerAlgorithmsByAlgorithmId(Guid algorithmId);

    /// <summary>
    /// Добавить относительное наименование пользовательского
    /// алгоритма для майнера.
    /// </summary>
    /// <param name="minerAlgorithm"> 
    /// Привязка относительного имени алгоритма к майнеру.
    /// </param>
    Task AddMinerAlgorithm(MinerAlgorithm minerAlgorithm);

    /// <summary>
    /// Удалить все привязки относительных наименований 
    /// алгоритмов к майнерам соответсвтующего алгоритма.
    /// </summary>
    /// <param name="algorithmId"> Идентификатор алгоритма. </param>
    Task RemoveAllMinerAlgorithmsById(Guid algorithmId);

    /// <summary>
    /// Редактировать привязки относительных наименований 
    /// алгоритма к майнерам соответствующего алгоритма.
    /// </summary>
    /// <param name="algorithmId"> Идентификатор алгоритма. </param>
    /// <param name="RelativeNames"> Список новых наименований. </param>
    /// <param name="minerIds"> Список идентификаторов майнеров. </param>
    Task EditMinerBindingsByAlgorithmId(Guid algorithmId,
                                        List<RelativeNameBindingModel> bindings);
}
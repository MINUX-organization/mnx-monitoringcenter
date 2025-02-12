using MNX.MonitoringCenter.Management.Core.Mining.Miner;
using MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm;

/// <summary>
/// Репозиторий для доступа к относительным наименованиям алгоритмов для майнеров.
/// </summary>
public interface IMinerAlgorithmRepository
{
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

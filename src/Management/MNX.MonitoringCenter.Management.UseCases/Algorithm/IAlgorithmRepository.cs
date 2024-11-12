namespace MNX.MonitoringCenter.Management.UseCases.Algorithm;

/// <summary>
/// Репозиторий для доступа к алгоритмам.
/// </summary>
public interface IAlgorithmRepository
{
    /// <summary>
    /// Получить доступные алгоритмы.
    /// </summary>
    /// <returns> Список алгоритмов. </returns>
    IAsyncEnumerable<Core.Algorithm> GetNamesOfAvailableAlgorithms(Specification specification);

    /// <summary>
    /// Получить алгоритм по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Алгоритм. </returns>
    Task<Core.Algorithm?> GetById(Guid id, CancellationToken cancellationToken = default);
}

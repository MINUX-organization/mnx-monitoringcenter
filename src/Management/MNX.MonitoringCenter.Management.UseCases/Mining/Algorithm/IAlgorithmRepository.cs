namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm;

/// <summary>
/// Репозиторий для доступа к алгоритмам.
/// </summary>
public interface IAlgorithmRepository
{
    /// <summary>
    /// Получить доступные алгоритмы.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Список алгоритмов. </returns>
    IAsyncEnumerable<Core.Mining.Algorithm> GetNamesOfAvailableAlgorithms(Specification specification);

    /// <summary>
    /// Получить алгоритм по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Алгоритм. </returns>
    Task<Core.Mining.Algorithm?> GetById(Guid id, CancellationToken cancellationToken = default);
}

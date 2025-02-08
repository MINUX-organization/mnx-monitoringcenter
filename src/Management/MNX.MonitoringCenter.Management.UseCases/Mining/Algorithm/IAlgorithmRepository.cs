namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm;

using Algorithm = Core.Mining.Algorithm;

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
    IAsyncEnumerable<Algorithm> GetNamesOfAvailableAlgorithms(Specification specification);

    /// <summary>
    /// Получить алгоритм по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор. </param>
    /// <param name="userId"> Идентификатор. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Алгоритм. </returns>
    Task<Algorithm?> GetById(Guid id,
                             Guid userId,
                             CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавить новый алгоритм.
    /// </summary>
    /// <param name="algorithm"> Алгоритм. </param>
    Task Add(Algorithm algorithm);

    /// <summary>
    /// Удалить пользовательский алгоритм по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор алгоритма. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    Task RemoveUsersAlgorithm(Guid id, Guid userId);

    /// <summary>
    /// Редактировать имя алгоритма.
    /// </summary>
    /// <param name="algorithmId"> Идентификатор алгоритма. </param>
    /// <param name="algorithmId"> Идентификатор пользователя. </param>
    /// <param name="newName"> Новое имя алгоритма. </param>
    Task EditAlgorithmName(Guid algorithmId, Guid userId, string newName);
}

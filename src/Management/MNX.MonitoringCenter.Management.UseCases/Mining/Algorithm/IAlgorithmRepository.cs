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
    IAsyncEnumerable<Algorithm> GetAvailable(Specification specification);

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
    /// Получить признак существования алгоритма по наименованию.
    /// </summary>
    /// <remarks>
    /// Доменные алгоритмы доступны всем пользователям и их UserId равен null.
    /// Пользовательские алгоритмы доступны только авторизированному 
    /// под данным UserId пользователю. Данный метод получает признак
    /// существования всех доступных пользователю алгоритмов,
    /// совпадающих с именем, подаваемым на вход данному методу.
    /// </remarks>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="name"> Наименование алгоритма. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Признак существования алгоритма. </returns>
    Task<bool> Exists(Guid userId,
                      string name,
                      CancellationToken cancellationToken);

    // TODO: Именование метода не отражает его суть.
    /// <summary>
    /// Получить признак существования алгоритма по наименованию,
    /// чей идентификатор не равен идентификатору алгоритма,
    /// передаваемому на вход.
    /// </summary>
    /// <param name="algorithmId"> Идентификатор алгоритма. </param>
    /// <param name="name"> Наименование алгоритма. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns>
    /// Признак существования алгоритма согласно условию.
    /// </returns>
    Task<bool> Exists(Guid algorithmId,
                      string name,
                      Guid userId,
                      CancellationToken cancellationToken);

    /// <summary>
    /// Добавить новый пользовательский алгоритм.
    /// </summary>
    /// <param name="algorithm"> Алгоритм. </param>
    Task AddAsync(Algorithm algorithm);

    /// <summary>
    /// Удалить пользовательский алгоритм по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор алгоритма. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    Task Remove(Guid id, Guid userId);

    /// <summary>
    /// Редактировать имя алгоритма.
    /// </summary>
    /// <param name="algorithmId"> Идентификатор алгоритма. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="newName"> Новое имя алгоритма. </param>
    Task EditAlgorithmName(Guid algorithmId, Guid userId, string newName);
}

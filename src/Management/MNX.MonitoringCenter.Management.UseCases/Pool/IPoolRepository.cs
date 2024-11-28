namespace MNX.MonitoringCenter.Management.UseCases.Pool;

using Pool = Core.Pool;

/// <summary>
/// Интерфейс репозитория, предоставляющего доступ к пулам
/// </summary>
public interface IPoolRepository
{
    /// <summary>
    /// Получить список всех пулов
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Список пулов </returns>
    IAsyncEnumerable<Pool> GetAllAvailable(Specification specification);

    /// <summary>
    /// Получить пул
    /// </summary>
    /// <param name="id"> Уникальный идентификатор </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Пул </returns>
    Task<Pool?> GetAvailableById(Guid id, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получить признак существование пула
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="domain"> Домен </param>
    /// <param name="port"> Порт </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns>
    /// <see langword="true"/>, если пул существует (оба параметра совпали), иначе <see langword="false"/>
    /// </returns>
    Task<bool> Exists(Guid userId, string domain, int port, CancellationToken cancellationToken);

    /// <summary>
    /// Добавить пул
    /// </summary>
    /// <param name="pool"> Пул </param>
    Task Add(Pool pool);

    /// <summary>
    /// Обновить пул
    /// </summary>
    /// <param name="pool"> Пул </param>
    Task Update(Pool pool);

    /// <summary>
    /// Удалить пул
    /// </summary>
    /// <param name="id"> Идентификатор пула. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    Task Remove(Guid id, Guid userId);
}

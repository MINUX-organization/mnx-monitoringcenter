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
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Список пулов </returns>
    IAsyncEnumerable<Pool> GetAllAvailable(Guid userId);

    /// <summary>
    /// Получить пул
    /// </summary>
    /// <param name="id"> Уникальный идентификатор </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Пул </returns>
    Task<Pool?> GetAvailableById(Guid id, Guid userId);

    /// <summary>
    /// Получить признак существование пула
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="domain"> Домен </param>
    /// <param name="port"> Порт </param>
    /// <returns>
    /// <see langword="true"/>, если пул существует (оба параметра совпали), иначе <see langword="false"/>
    /// </returns>
    Task<bool> Exists(Guid userId, string domain, int port);

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
    /// <param name="pool"> Пул </param>
    Task Remove(Pool pool);
}

using MINUX.Backend.Worker.Core;

namespace MINUX.Backend.Worker.UseCases.Abstractions;

/// <summary>
/// Интерфейс репозитория, предоставляющего доступ к пулам
/// </summary>
public interface IPoolRepository
{
    /// <summary>
    /// Получить список всех пулов
    /// </summary>
    /// <returns> Список пулов </returns>
    IAsyncEnumerable<Pool> GetAll();

    /// <summary>
    /// Получить пул
    /// </summary>
    /// <param name="id"> Уникальный идентификатор </param>
    /// <returns> Пул </returns>
    Task<Pool?> GetById(Guid id);

    /// <summary>
    /// Получить признак существование пула
    /// </summary>
    /// <param name="domain"> Домен </param>
    /// <param name="port"> Порт </param>
    /// <returns> <see langword="true"/>, если пул существует (оба параметра совпали), иначе <see langword="false"/> </returns>
    Task<bool> Exists(string domain, int port);

    /// <summary>
    /// Добавить пул
    /// </summary>
    /// <param name="pool"> Пул </param>
    /// <returns> Уникальный идентификатор </returns>
    Task<Guid> Add(Pool pool);

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

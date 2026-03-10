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
    /// Проверить существование майнера по id.
    /// </summary>
    /// <param name="id"> Идентификатор. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Признак существования майнера. </returns>
    Task<bool> Exists(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Проверить существование майнера по идентификатору пользователя и наименованию майнера.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="minerName"> Имя майнера. </param>
    /// <param name="minerVersion"> Версия майнера. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Признак существования майнера. </returns>
    Task<bool> Exists(Guid userId, string minerName, string minerVersion, CancellationToken cancellationToken);

    // TODO: По именованию метода операция не является очевидной в данном методе.
    /// <summary>
    /// Проверить существование майнера по идентификатору пользователя и наименованию майнера,
    /// исключая майнер с идентификатором, равным minerId.
    /// </summary>
    /// <param name="minerId"> Идентификатор майнера. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="minerName"> Наименование майнера.</param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Признак существования майнера. </returns>
    Task<bool> Exists(Guid minerId, Guid userId, string minerName, CancellationToken cancellationToken);

    /// <summary>
    /// Добавить майнер.
    /// </summary>
    /// <param name="miner"> Майнер. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task Add(Miner miner, CancellationToken cancellationToken);

    /// <summary>
    /// Редактировать майнер.
    /// </summary>
    /// <param name="miner"></param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task Edit(Miner miner, CancellationToken cancellationToken);

    /// <summary>
    /// Удалить майнер.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task Remove(Guid id, Guid userId, CancellationToken cancellationToken);
}
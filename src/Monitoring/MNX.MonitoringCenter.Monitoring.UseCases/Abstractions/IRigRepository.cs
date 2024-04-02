using MNX.MonitoringCenter.Monitoring.Core;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

/// <summary>
/// Интерфейс репозитория для доступа к данным ригов.
/// </summary>
public interface IRigRepository
{
    /// <summary>
    /// Получить риг по идентификатору.
    /// </summary>
    /// <param name="id">  Уникальный идентификатор. </param>
    /// <returns> Риг </returns>
    Task<Rig?> GetById(Guid id, long userId);

    /// <summary>
    /// Получить список всех ригов клиента.
    /// </summary>
    /// <returns> Список ригов. </returns>
    IAsyncEnumerable<Rig> GetAvailable(long userId);

    /// <summary>
    /// Добавить риг.
    /// </summary>
    /// <param name="rig"> Риг </param>
    /// <returns> Уникальный идентификатор рига. </returns>
    Task<Guid> Add(Rig rig);

    /// <summary>
    /// Обновить данные о риге.
    /// </summary>
    /// <param name="rig"> Риг с новыми данными </param>
    Task Update(Rig rig);

    /// <summary>
    /// Удалить риг.
    /// </summary>
    /// <param name="rig"> Риг </param>
    Task Remove(Rig rig);
}

using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries;

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
    /// <returns> Риг. </returns>
    Task<Rig?> GetById(Guid id, long userId);

    /// <summary>
    /// Получить список ригов.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Список ригов. </returns>
    IAsyncEnumerable<Rig> GetList(Specification specification);

    /// <summary>
    /// Получить обобщённые количественные данные ригов.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Обобщённые количественные данные ригов. </returns>
    Task<RigsSummarizedQuantitativeData> GetRigsSummarizedQuantitativeData(Specification specification);

    /// <summary>
    /// Получить список идентификаторов ригов.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Список идентификаторов ригов. </returns>
    Task<IEnumerable<Guid>> GetIds(Specification specification);

    /// <summary>
    /// Добавить риг.
    /// </summary>
    /// <param name="rig"> Риг. </param>
    /// <returns> Уникальный идентификатор рига. </returns>
    Task<Guid> Add(Rig rig);

    /// <summary>
    /// Обновить данные о риге.
    /// </summary>
    /// <param name="rig"> Риг с новыми данными. </param>
    Task Update(Rig rig);

    /// <summary>
    /// Удалить риг.
    /// </summary>
    /// <param name="rig"> Риг. </param>
    Task Remove(Rig rig);
}

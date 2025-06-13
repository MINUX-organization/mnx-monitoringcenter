using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.Core.Services;

/// <summary>
/// Репозиторий для доступа к ригам.
/// </summary>
public interface IRigRepository
{
    /// <summary>
    /// Получить список доступных ригов.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Список ригов. </returns>
    IAsyncEnumerable<Rig> GetAvailable(Guid userId);

    /// <summary>
    /// Получить риг по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор рига. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Риг. </returns>
    Task<Rig?> GetById(RigId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавить риг.
    /// </summary>
    /// <param name="rig"> Риг. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task Add(Rig rig, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить риг.
    /// </summary>
    /// <param name="rig"> Риг. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task Update(Rig rig, CancellationToken cancellationToken = default);
}

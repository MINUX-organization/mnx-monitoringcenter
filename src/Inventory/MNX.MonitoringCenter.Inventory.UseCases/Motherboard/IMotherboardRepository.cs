namespace MNX.MonitoringCenter.Inventory.UseCases.Motherboard;

/// <summary>
/// Репозиторий для доступа к материнским платам.
/// </summary>
public interface IMotherboardRepository
{
    /// <summary>
    /// Получить материнскую плату по идентификатору рига.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Материнская плата. </returns>
    Task<Contracts.Motherboard.Motherboard?> GetByRigId(Guid rigId, Guid userId,
                                                        CancellationToken cancellationToken);
}

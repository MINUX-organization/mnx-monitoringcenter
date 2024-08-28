namespace MNX.MonitoringCenter.Inventory.UseCases.NetworkAdapter;

/// <summary>
/// Репозиторий для доступа к сетевым адаптерам.
/// </summary>
public interface INetworkAdapterRepository
{
    /// <summary>
    /// Получить список сетевых адаптеров, подключенных к ригу.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Список сетевых адаптеров. </returns>
    Task<List<Contracts.NetworkAdapter.NetworkAdapter>?> GetList(Guid rigId, CancellationToken cancellationToken);
}

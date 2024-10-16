using MNX.MonitoringCenter.Inventory.Contracts.Requests;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.NetworkAdapter;

using NetworkAdapter = Contracts.NetworkAdapter.NetworkAdapter;

/// <summary>
/// Репозиторий для доступа к сетевым адаптерам.
/// </summary>
public interface INetworkAdapterRepository
{
    /// <summary>
    /// Получить список сетевых адаптеров, подключенных к ригу.
    /// </summary>
    /// <param name="specification"> Спецификация инвентаризации. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Список сетевых адаптеров. </returns>
    Task<List<NetworkAdapter>?> GetNetworkAdapters(InventorySpecification specification,
                                                   CancellationToken cancellationToken);
}

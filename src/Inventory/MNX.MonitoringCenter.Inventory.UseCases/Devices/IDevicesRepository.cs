using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices;

namespace MNX.MonitoringCenter.Inventory.UseCases.Devices;

/// <summary>
/// Репозиторий для доступа к устройствам.
/// </summary>
public interface IDevicesRepository
{
    /// <summary>
    /// Получить список устройств.
    /// </summary>
    /// <param name="specification"> Спецификация инвентаризации. </param>
    /// <param name="devicesTypes"> Типы запрашиваемых устройств. </param>
    /// <returns> Поток устройств. </returns>
    IAsyncEnumerable<Device> GetList(InventorySpecification specification, DeviceType? devicesTypes);
}

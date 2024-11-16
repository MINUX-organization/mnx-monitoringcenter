using MNX.MonitoringCenter.Management.Contracts.MiningDevice;

namespace MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetMiningDevices;

/// <summary>
/// Группа майнинг устройств.
/// </summary>
public class MiningDevicesGroup
{
    /// <summary>
    /// Название группы.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Список майнинг устройств.
    /// </summary>
    public List<MiningDeviceModel> MiningDevices { get; set; } = new(0);
}

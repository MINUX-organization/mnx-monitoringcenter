using MNX.MonitoringCenter.Management.Core.Mining.Miner;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Contracts.MinerContracts;

public class MinerInputModel
{
    public string Name { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public List<MinerAlgorithm> SupportedAlgorithms { get; set; } = new(0);

    public DeviceTypeManufacturerCombination SupportedDevices { get; set; }

    public MiningModeEnum MiningMode { get; init; } = MiningModeEnum.Single;

    public MinerInputModel(string name,
                           string version,
                           List<MinerAlgorithm> supportedAlgorithms,
                           DeviceTypeManufacturerCombination supportedDevices,
                           MiningModeEnum miningMode)
    {
        Name = name;
        Version = version;
        SupportedAlgorithms = supportedAlgorithms;
        SupportedDevices = supportedDevices;
        MiningMode = miningMode;
    }
}

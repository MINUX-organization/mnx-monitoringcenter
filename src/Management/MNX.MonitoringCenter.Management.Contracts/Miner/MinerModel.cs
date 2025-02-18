using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Contracts.Miner;

public class MinerModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Version { get; set; }
    public MinerTypeEnum Type { get; set; }
    public DeviceTypeManufacturerCombination SupportedDevices { get; set; }
    public List<MinerAlgorithmModel> Algorithms { get; set; }
    public Guid? OwnerId { get; set; }
    public string? InstallationUrl { get; set; }
    public string? PoolTemplate { get; set; }
    public string? WalletWorkerTemplate { get; set; }
    public MiningModeEnum MiningMode { get; set; }
}
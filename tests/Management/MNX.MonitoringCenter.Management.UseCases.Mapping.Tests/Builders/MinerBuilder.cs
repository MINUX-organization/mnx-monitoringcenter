using MNX.MonitoringCenter.Management.Core.Mining.Miner;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders;

using Miner = Core.Mining.Miner.Miner;

public class MinerBuilder
{
    private static int _counter = 1;

    private Guid _id = Guid.NewGuid();
    private string _name = $"Miner_{_counter}";
    private string _installationUrl = $"www.install_{_counter}.com";
    private string _version = $"1.0.{_counter++}";
    private MiningModeEnum _miningMode = MiningModeEnum.Single;
    private Guid? _ownerId = null;
    private MinerTypeEnum _minerType => _ownerId is null ? MinerTypeEnum.Integrated : MinerTypeEnum.Custom;
    private DeviceTypeManufacturerCombination _supportedDevices = DeviceTypeManufacturerCombination.None;
    private List<MinerAlgorithm> _supportedAlgorithms = [];
    private string? _walletWorkerTemplate = null;
    private string? _poolTemplate = null;

    public MinerBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public MinerBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public MinerBuilder WithInstallationUrl(string installationUrl)
    {
        _installationUrl = installationUrl;
        return this;
    }

    public MinerBuilder WithVersion(string version)
    {
        _version = version;
        return this;
    }

    public MinerBuilder WithMiningMode(MiningModeEnum miningMode)
    {
        _miningMode = miningMode;
        return this;
    }

    public MinerBuilder WithOwner(Guid? ownerId = null)
    {
        _ownerId = ownerId;
        return this;
    }

    public MinerBuilder WithSupportedDevices(DeviceTypeManufacturerCombination supportedDevices)
    {
        _supportedDevices = supportedDevices;
        return this;
    }

    public MinerBuilder WithWalletWorkerTemplate(string walletWorkerTemplate)
    {
        _walletWorkerTemplate = walletWorkerTemplate;
        return this;
    }

    public MinerBuilder WithPoolTemplate(string poolTemplate)
    {
        _poolTemplate = poolTemplate;
        return this;
    }

    public MinerBuilder AddAlgorithm(Action<MinerAlgorithmBuilder> configure)
    {
        var builder = new MinerAlgorithmBuilder();
        configure(builder);
        _supportedAlgorithms.Add(builder.Build());
        return this;
    }

    public Miner Build()
    {
        return new Miner
        {
            Id = _id,
            Name = _name,
            InstallationUrl = _installationUrl,
            Version = _version,
            MiningMode = _miningMode,
            OwnerId = _ownerId,
            Type = _minerType,
            SupportedDevices = _supportedDevices,
            SupportedAlgorithms = _supportedAlgorithms,
            WalletWorkerTemplate = _walletWorkerTemplate,
            PoolTemplate = _poolTemplate,
        };
    }
}

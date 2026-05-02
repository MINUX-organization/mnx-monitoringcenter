using MNX.MonitoringCenter.Management.Core.Mining.Miner;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

public class MinerBuilder
{
    private static int _counter = 1;
    private MinerTypeEnum MinerType => _ownerId is null ? MinerTypeEnum.Integrated : MinerTypeEnum.Custom;

    protected Guid _id = Guid.NewGuid();
    protected string _name = $"Miner_{_counter}";
    protected string _installationUrl = $"www.install_{_counter}.com";
    protected string _version = $"1.0.{_counter++}";
    protected MiningModeEnum _miningMode = MiningModeEnum.Single;
    protected Guid? _ownerId = null;
    protected DeviceTypeManufacturerCombination _supportedDevices = DeviceTypeManufacturerCombination.None;
    protected List<MinerAlgorithm> _supportedAlgorithms = [];
    protected string? _walletWorkerTemplate = null;
    protected string? _poolTemplate = null;

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
        _ownerId = ownerId ?? Guid.NewGuid();
        return this;
    }

    public MinerBuilder WithSupportedDevices(DeviceTypeManufacturerCombination supportedDevices)
    {
        _supportedDevices = supportedDevices;
        return this;
    }

    public MinerBuilder WithWalletWorkerTemplate(string? walletWorkerTemplate)
    {
        _walletWorkerTemplate = walletWorkerTemplate;
        return this;
    }

    public MinerBuilder WithPoolTemplate(string? poolTemplate)
    {
        _poolTemplate = poolTemplate;
        return this;
    }

    public MinerBuilder AddAlgorithm(Func<MinerAlgorithmBuilder, MinerAlgorithmBuilder>? configure = null)
    {
        var builder = new MinerAlgorithmBuilder();
        builder = configure?.Invoke(builder) ?? builder;
        _supportedAlgorithms.Add(builder
            .WithMinerId(_id)
            .Build());
        return this;
    }

    public MinerBuilder AddAlgorithm(MinerAlgorithm algorithm)
    {
        _supportedAlgorithms.Add(algorithm);
        return this;
    }

    public MinerBuilder WithAlgorithms(Func<List<MinerAlgorithm>> factory)
    {
        _supportedAlgorithms.Clear();
        _supportedAlgorithms.AddRange(factory());
        return this;
    }

    public MinerBuilder WithAlgorithms(List<MinerAlgorithm> algorithms)
    {
        _supportedAlgorithms.Clear();
        _supportedAlgorithms.AddRange(algorithms);
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
            Type = MinerType,
            SupportedDevices = _supportedDevices,
            SupportedAlgorithms = _supportedAlgorithms,
            WalletWorkerTemplate = _walletWorkerTemplate,
            PoolTemplate = _poolTemplate,
        };
    }
}

using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;

public class MinerInputModelBuilder
{
    private static int _counter = 1;

    protected string _name = $"MinerInputModelName_{_counter}";
    protected string _version = $"1.0.{_counter}";
    protected string _installUrl = $"www.install-miner-input-{_counter}.com";
    protected DeviceTypeManufacturerCombination _supportedDevices = DeviceTypeManufacturerCombination.None;
    protected string _poolTemplate = $"PoolTemplateMinerInput{_counter}";
    protected string _walletWorkerTemplate = $"WalletWorkerTemplateMinerInput{_counter++}";
    protected MiningModeEnum _miningMode = MiningModeEnum.Single;

    public MinerInputModelBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    
    public MinerInputModelBuilder WithVersion(string version)
    {
        _version = version;
        return this;
    }
    
    public MinerInputModelBuilder WithInstallUrl(string installUrl)
    {
        _installUrl = installUrl;
        return this;
    }

    public MinerInputModelBuilder WithSupportedDevices(DeviceTypeManufacturerCombination supportedDevices)
    {
        _supportedDevices = supportedDevices;
        return this;
    }

    public MinerInputModelBuilder WithPoolTemplate(string poolTemplate)
    {
        _poolTemplate = poolTemplate;
        return this;
    }

    public MinerInputModelBuilder WithWalletWorkerTemplate(string walletWorkerTemplate)
    {
        _walletWorkerTemplate = walletWorkerTemplate;
        return this;
    }

    public MinerInputModelBuilder WithMiningMode(MiningModeEnum miningMode)
    {
        _miningMode = miningMode;
        return this;
    }

    public MinerInputModel Build() => new(_name,
                                          _version,
                                          _installUrl,
                                          _supportedDevices,
                                          _poolTemplate,
                                          _walletWorkerTemplate,
                                          _miningMode);
}

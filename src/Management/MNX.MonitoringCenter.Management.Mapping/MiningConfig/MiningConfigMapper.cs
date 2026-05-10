using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.MiningConfig;

/// <summary>
/// Реализация <see cref="IMiningConfigMapper"/>.
/// </summary>
public class MiningConfigMapper : IMiningConfigMapper
{
    private readonly IPoolMapper _poolMapper;

    private readonly IWalletMapper _walletMapper;

    ///
    public MiningConfigMapper(IPoolMapper poolMapper, IWalletMapper walletMapper)
    {
        _poolMapper = poolMapper ?? throw new ArgumentNullException(nameof(poolMapper));
        _walletMapper = walletMapper ?? throw new ArgumentNullException(nameof(walletMapper));
    }

    /// <inheritdoc/>
    public BaseMiningConfig MapToCoreEntity(MiningConfigInputModel model)
    {
        if (model.DeviceType == MiningDeviceType.CPU)
        {
            var config = (CpuMiningConfigInputModel)model;
            var coinConfigs = MapInputCoinConfigsToCore(model.CoinConfigs);

            return new CpuMiningConfig()
            {
                CoinConfigs = coinConfigs,
                AdditionalArguments = config.AdditionalArguments,
                ConfigFileContent = config.ConfigFileContent,
                HugePages = config.HugePages,
                ThreadsCount = config.ThreadsCount,
            };
        }
        if (model.DeviceType == MiningDeviceType.GPU)
        {
            var config = (GpuMiningConfigInputModel)model;
            var coinConfigs = MapInputCoinConfigsToCore(model.CoinConfigs);

            return new GpuMiningConfig()
            {
                CoinConfigs = coinConfigs,
                AdditionalArguments = config.AdditionalArguments,
                ConfigFileContent = config.ConfigFileContent,
            };
        }

        throw new NotSupportedException($"Device type {model.DeviceType} is not supported");
    }

    /// <inheritdoc/>
    public BaseMiningConfigModel MapToModel(BaseMiningConfig model)
    {
        if (model.DeviceType == MiningDeviceType.CPU)
        {
            var config = (CpuMiningConfig)model;
            var coinConfigs = MapCoinConfigsToModel(config.CoinConfigs);

            return new CpuMiningConfigModel()
            {
                CoinConfigs = coinConfigs,
                AdditionalArguments = config.AdditionalArguments,
                ConfigFileContent = config.ConfigFileContent,
                HugePages = config.HugePages,
                ThreadsCount = config.ThreadsCount
            };
        }
        if (model.DeviceType == MiningDeviceType.GPU)
        {
            var config = (GpuMiningConfig)model;
            var coinConfigs = MapCoinConfigsToModel(config.CoinConfigs);

            return new GpuMiningConfigModel()
            {
                CoinConfigs = coinConfigs,
                AdditionalArguments = config.AdditionalArguments,
                ConfigFileContent = config.ConfigFileContent
            };
        }

        throw new NotImplementedException($"Device type {model.DeviceType} is not supported");
    }

    private static List<MiningCoinConfig> MapInputCoinConfigsToCore(List<MiningCoinConfigInputModel> models)
    {
        var coinConfigs = new List<MiningCoinConfig>();
        foreach (var model in models)
        {
            var coinConfig = new MiningCoinConfig()
            {
                PoolId = model.PoolId,
                WalletId = model.WalletId,
                PoolPassword = model.PoolPassword
            };
            coinConfigs.Add(coinConfig);
        }

        return coinConfigs;
    }

    private List<MiningCoinConfigModel> MapCoinConfigsToModel(List<MiningCoinConfig> models)
    {
        var coinConfigs = new List<MiningCoinConfigModel>();
        foreach (var model in models)
        {
            var poolModel = _poolMapper.MapToModel(model.Pool!);
            var walletModel = _walletMapper.MapToModel(model.Wallet!);

            var coinConfig = new MiningCoinConfigModel()
            {
                Wallet = walletModel,
                Pool = poolModel,
                PoolPassword = model.PoolPassword,
            };

            coinConfigs.Add(coinConfig);
        }

        return coinConfigs;
    }
}

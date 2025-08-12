using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings.Models;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Events;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.AgentCommands;

/// <summary>
/// Реализация <see cref="IAgentCommandsMapper"/>.
/// </summary>
public class AgentCommandsMapper : IAgentCommandsMapper
{
    private readonly IPoolMapper _poolMapper;

    private readonly IWalletMapper _walletMapper;

    ///
    public AgentCommandsMapper(IPoolMapper poolMapper, IWalletMapper walletMapper)
    {
        _poolMapper = poolMapper ?? throw new ArgumentNullException(nameof(poolMapper));
        _walletMapper = walletMapper ?? throw new ArgumentNullException(nameof(walletMapper));
    }

    /// <inheritdoc/>
    public List<WorkerSettings> MapToWorkerSettings(List<DeviceFLightSheet> models)
    {
        var workerSettings = new List<WorkerSettings>();

        foreach (var model in models)
        {
            var settigsModel = MapTargetToSettings(model.FlightSheet);
            var worker = new WorkerSettings()
            {
                WorkerId = model.Device.Id,
                SettingsModel = settigsModel
            };
            workerSettings.Add(worker);
        }

        return workerSettings;
    }

    /// <summary>
    /// Преобразовать сущность <see cref="FlightSheetTarget"/> в 
    /// реализацию сущности <see cref="BaseMiningSettingsModel"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns> Новый экземпляр реализации <see cref="BaseMiningSettingsModel"/>. </returns>
    /// <exception cref="NotSupportedException">
    /// Выбрасывается при некорректном типе девайса у полетного листа.
    /// </exception>
    private BaseMiningSettingsModel? MapTargetToSettings(FlightSheetTarget? model)
    {
        if (model == null) return null;

        if (model.DeviceType == MiningDeviceType.CPU)
        {
            var config = (CpuMiningConfig)model.MiningConfig;
            var coinConfigs = MapCoinConfigsToModel(model.MiningConfig.CoinConfigs);
            return new CpuMiningSettingsModel()
            {
                MinerName = model.Miner!.Name,
                MinerVersion = model.Miner!.Version,
                AdditionalArguments = config.AdditionalArguments,
                ConfigFileContent = config.ConfigFileContent,
                HugePages = config.HugePages,
                ThreadsCount = config.ThreadsCount,
                CoinConfigs = coinConfigs
            };
        }
        if (model.DeviceType == MiningDeviceType.GPU)
        {
            var config = (GpuMiningConfig)model.MiningConfig;
            var coinConfigs = MapCoinConfigsToModel(model.MiningConfig.CoinConfigs);
            return new GpuMiningSettingsModel()
            {
                MinerName = model.Miner!.Name,
                MinerVersion= model.Miner!.Version,
                AdditionalArguments = config.AdditionalArguments,
                ConfigFileContent = config.ConfigFileContent,
                CoinConfigs = coinConfigs
            };
        }

        throw new NotSupportedException($"Device type {model.DeviceType} does not support");
    }

    /// <summary>
    /// Преобразовать коллекцию <see cref="MiningCoinConfig"/>
    /// в коллекцию <see cref="MiningCoinConfigModel"/>.
    /// </summary>
    /// <param name="models"> Модель данных. </param>
    /// <returns> Новая коллекция <see cref="MiningCoinConfigModel"/>. </returns>
    private List<MiningCoinConfigModel> MapCoinConfigsToModel(List<MiningCoinConfig> models)
    {
        var coinConfigs = new List<MiningCoinConfigModel>();
        foreach (var model in models)
        {
            var poolModel = _poolMapper.MapToModel(model.Pool!);
            var walletModel = _walletMapper.MapToModel(model.Wallet!);

            var algorithmName = model.Wallet!.Cryptocurrency!.Algorithm!.Name;

            var coinConfig = new MiningCoinConfigModel()
            {
                WalletAddress = walletModel.Address,
                AlgorithmName = algorithmName,
                PoolHost = poolModel.Domain,
                PoolPort = poolModel.Port,
                Tls = poolModel.Tls,
                PoolPassword = model.PoolPassword
            };

            coinConfigs.Add(coinConfig);
        }

        return coinConfigs;
    }
}

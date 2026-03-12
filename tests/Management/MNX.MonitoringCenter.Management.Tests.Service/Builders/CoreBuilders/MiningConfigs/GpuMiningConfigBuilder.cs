using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningConfigs;

public class GpuMiningConfigBuilder : MiningConfigBuilder<GpuMiningConfigBuilder, GpuMiningConfig>
{
    public override GpuMiningConfig Build()
    {
        return new GpuMiningConfig()
        {
            AdditionalArguments = _additionalArguments,
            ConfigFileContent = _configFileContent,
            CoinConfigs = _coinConfigs,
        };
    }
}

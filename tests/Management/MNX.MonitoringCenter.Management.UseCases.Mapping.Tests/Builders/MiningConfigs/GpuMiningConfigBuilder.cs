using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.MiningConfigs;

public class GpuMiningConfigBuilder : MiningConfigBuilder<GpuMiningConfigBuilder, GpuMiningConfig>
{
    public override GpuMiningConfig Build()
    {
        var entity = new GpuMiningConfig();
        ApplyBasesProperties(entity);
        return entity;
    }
}

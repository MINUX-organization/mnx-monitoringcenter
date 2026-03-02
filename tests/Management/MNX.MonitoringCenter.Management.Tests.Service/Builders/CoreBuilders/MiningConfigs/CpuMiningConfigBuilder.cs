using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningConfigs;

public class CpuMiningConfigBuilder : MiningConfigBuilder<CpuMiningConfigBuilder, CpuMiningConfig>
{
    protected int? _threads = 1;
    protected int? _hugePages = 0;

    public CpuMiningConfigBuilder WithThreads(int? threads)
    {
        _threads = threads;
        return this;
    }

    public CpuMiningConfigBuilder WithHugePages(int? hugePages)
    {
        _hugePages = hugePages;
        return this;
    }

    public override CpuMiningConfig Build()
    {
        var entity = new CpuMiningConfig()
        {
            ThreadsCount = _threads,
            HugePages = _hugePages
        };

        ApplyBaseProperties(entity);

        return entity;
    }
}

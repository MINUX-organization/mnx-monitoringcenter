using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.MiningConfigModels;

public class CpuMiningConfigModelBuilder :
    BaseMiningConfigModelBuilder<CpuMiningConfigModelBuilder, CpuMiningConfigModel>
{
    protected int? _threadsCount = null;
    protected int? _hugePages = null;

    public CpuMiningConfigModelBuilder WithThreadsCount(int? threadsCount)
    {
        _threadsCount = threadsCount;
        return this;
    }

    public CpuMiningConfigModelBuilder WithHugePages(int? hugePages)
    {
        _hugePages = hugePages;
        return this;
    }

    public override BaseMiningConfigModel Build()
    {
        return new CpuMiningConfigModel
        {
            AdditionalArguments = _additionalArguments,
            ConfigFileContent = _configFileContent,
            HugePages = _hugePages,
            ThreadsCount = _threadsCount,
            CoinConfigs = _coinConfigs
        };
    }
}

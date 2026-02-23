using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.ContractBuilders.BaseMiningConfigModels;

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
        var (additionalArguments, configFileContent, coinConfigs) = GetBaseData();
        return new CpuMiningConfigModel
        {
            AdditionalArguments = additionalArguments,
            ConfigFileContent = configFileContent,
            HugePages = _hugePages,
            ThreadsCount = _threadsCount,
            CoinConfigs = coinConfigs
        };
    }
}

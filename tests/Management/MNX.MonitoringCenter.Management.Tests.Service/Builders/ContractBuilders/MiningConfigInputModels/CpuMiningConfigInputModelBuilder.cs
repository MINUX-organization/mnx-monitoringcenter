using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.MiningConfigInputModels;

public class CpuMiningConfigInputModelBuilder :
    MiningConfigInputModelBuilder<CpuMiningConfigInputModelBuilder, CpuMiningConfigInputModel>
{
    protected int? _threadsCount = null;
    protected int? _hugePages = null;

    public CpuMiningConfigInputModelBuilder WithHugePages(int? hugePages)
    {
        _hugePages = hugePages;
        return this;
    }

    public CpuMiningConfigInputModelBuilder WithThreadsCount(int? threadsCount)
    {
        _threadsCount = threadsCount;
        return this;
    }

    public override MiningConfigInputModel Build()
    {
        var (additionalArgument, configFileContent, coinConfigs) = GetBaseValues();
        return new CpuMiningConfigInputModel()
        {
            AdditionalArguments = additionalArgument,
            ConfigFileContent = configFileContent,
            CoinConfigs = coinConfigs,
            HugePages = _hugePages,
            ThreadsCount = _threadsCount
        };
    }
}

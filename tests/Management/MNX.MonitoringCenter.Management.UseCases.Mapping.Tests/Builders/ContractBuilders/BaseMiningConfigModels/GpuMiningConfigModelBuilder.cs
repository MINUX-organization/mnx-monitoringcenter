using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.ContractBuilders.BaseMiningConfigModels;

internal class GpuMiningConfigModelBuilder :
    BaseMiningConfigModelBuilder<GpuMiningConfigModelBuilder, GpuMiningConfigModel>
{
    public override BaseMiningConfigModel Build()
    {
        var (additionalArguments, configFileContent, coinConfigs) = GetBaseData();
        return new GpuMiningConfigModel
        {
            AdditionalArguments = additionalArguments,
            ConfigFileContent = configFileContent,
            CoinConfigs = coinConfigs
        };
    }
}

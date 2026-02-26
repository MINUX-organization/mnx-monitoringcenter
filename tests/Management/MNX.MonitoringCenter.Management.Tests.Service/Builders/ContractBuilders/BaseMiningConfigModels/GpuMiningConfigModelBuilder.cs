using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.BaseMiningConfigModels;

public class GpuMiningConfigModelBuilder :
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

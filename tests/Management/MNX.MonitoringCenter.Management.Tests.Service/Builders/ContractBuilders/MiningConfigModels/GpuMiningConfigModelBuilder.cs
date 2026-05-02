using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.MiningConfigModels;

public class GpuMiningConfigModelBuilder :
    BaseMiningConfigModelBuilder<GpuMiningConfigModelBuilder, GpuMiningConfigModel>
{
    public override BaseMiningConfigModel Build()
    {
        return new GpuMiningConfigModel
        {
            AdditionalArguments = _additionalArguments,
            ConfigFileContent = _configFileContent,
            CoinConfigs = _coinConfigs
        };
    }
}

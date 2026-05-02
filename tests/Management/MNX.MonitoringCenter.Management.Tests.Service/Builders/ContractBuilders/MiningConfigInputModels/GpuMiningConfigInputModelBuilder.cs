using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.MiningConfigInputModels;

public class GpuMiningConfigInputModelBuilder :
    MiningConfigInputModelBuilder<GpuMiningConfigInputModelBuilder, GpuMiningConfigInputModel>
{
    public override MiningConfigInputModel Build()
    {
        return new GpuMiningConfigInputModel
        {
            AdditionalArguments = _additionalArguments,
            ConfigFileContent = _configFileContent,
            CoinConfigs = _coinConfigs
        };
    }
}

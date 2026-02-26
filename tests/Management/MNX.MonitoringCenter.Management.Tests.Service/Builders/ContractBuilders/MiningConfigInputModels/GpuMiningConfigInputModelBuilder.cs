using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.MiningConfigInputModels;

public class GpuMiningConfigInputModelBuilder :
    MiningConfigInputModelBuilder<GpuMiningConfigInputModelBuilder, GpuMiningConfigInputModel>
{
    public override MiningConfigInputModel Build()
    {
        var (additionalArguments, configFileContent, coinConfigs) = GetBaseValues();

        return new GpuMiningConfigInputModel
        {
            AdditionalArguments = additionalArguments,
            ConfigFileContent = configFileContent,
            CoinConfigs = coinConfigs
        };
    }
}

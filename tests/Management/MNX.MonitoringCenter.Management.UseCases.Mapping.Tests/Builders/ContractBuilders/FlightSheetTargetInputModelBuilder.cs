using MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.ContractBuilders.MiningConfigInputModels;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.ContractBuilders;

public class FlightSheetTargetInputModelBuilder
{
    private MiningConfigInputModel? _miningConfig = null;
    private Guid _minerId = Guid.NewGuid();

    public FlightSheetTargetInputModelBuilder WithMiningConfig(Func<MiningConfigInputModel> factory)
    {
        _miningConfig = factory();
        return this;
    }

    public FlightSheetTargetInputModelBuilder WithMinerId(Guid minerId)
    {
        _minerId = minerId;
        return this;
    }

    public FlightSheetTargetInputModel Build()
    {
        return new FlightSheetTargetInputModel
        {
            MinerId = _minerId,
            MiningConfig = _miningConfig ?? new GpuMiningConfigInputModelBuilder().Build()
        };
    }
}

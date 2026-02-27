using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings.Fan;

public abstract class BaseFanOverclockingModelBuilder<TBuilder, TModel>
    where TBuilder : BaseFanOverclockingModelBuilder<TBuilder, TModel>
    where TModel : IFanOverclockingModel
{
    public abstract IFanOverclockingModel Build();
}

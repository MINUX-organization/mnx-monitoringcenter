using MNX.MonitoringCenter.Management.Contracts.Overclocking;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings;

public abstract class BaseOverclockingModelBuilder<TBuilder, TEntity>
    where TBuilder : BaseOverclockingModelBuilder<TBuilder, TEntity>
    where TEntity : IOverclockingModel
{
    public abstract IOverclockingModel Build();
}

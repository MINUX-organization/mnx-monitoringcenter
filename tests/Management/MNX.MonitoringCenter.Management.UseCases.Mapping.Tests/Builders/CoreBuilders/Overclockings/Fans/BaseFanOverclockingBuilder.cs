using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.CoreBuilders.Overclockings.Fans;

public abstract class BaseFanOverclockingBuilder<TBuilder, TEntity>
    where TBuilder : BaseFanOverclockingBuilder<TBuilder, TEntity>
    where TEntity : IFanOverclocking, new()
{
    protected Guid _id = Guid.NewGuid();

    public TBuilder WithId(Guid id)
    {
        _id = id;
        return (TBuilder)this;
    }

    public abstract IFanOverclocking Build();
}

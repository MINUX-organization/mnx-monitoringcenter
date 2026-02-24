using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.CoreBuilders.Overclockings;

public abstract class BaseOverclockingBuilder<TBuilder, TEntity>
    where TBuilder : BaseOverclockingBuilder<TBuilder, TEntity>
    where TEntity : IOverclocking
{
    protected Guid _id = Guid.NewGuid();

    public TBuilder WithId(Guid id)
    {
        _id = id;
        return (TBuilder)this;
    }

    public abstract IOverclocking Build();
}

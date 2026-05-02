using MNX.MonitoringCenter.Management.Core.Mining;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

public class AlgorithmBuilder
{
    private static int _counter = 1;

    protected Guid _id = Guid.NewGuid();
    protected string _name = $"Algorithm_{_counter++}";
    protected Guid? _ownerId = null;

    public AlgorithmBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public AlgorithmBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public AlgorithmBuilder WithOwner(Guid? ownerId = null)
    {
        _ownerId = ownerId ?? Guid.NewGuid();
        return this;
    }

    public Algorithm Build()
    {
        return new Algorithm
        {
            Id = _id,
            Name = _name,
            OwnerId = _ownerId
        };
    }
}

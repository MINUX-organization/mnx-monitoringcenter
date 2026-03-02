using MNX.MonitoringCenter.Management.Core.Mining.Miner;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

public class MinerAlgorithmBuilder
{
    private static int _counter = 1;

    protected string _name = $"MinerAlgorithm_{_counter++}";
    protected Guid _algorithmId = Guid.NewGuid();
    protected Guid _minerId = Guid.NewGuid();

    public MinerAlgorithmBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public MinerAlgorithmBuilder WithAlgorithmId(Guid algorithmId)
    {
        _algorithmId = algorithmId;
        return this;
    }

    public MinerAlgorithmBuilder WithMinerId(Guid minerId)
    {
        _minerId = minerId;
        return this;
    }

    public MinerAlgorithm Build()
    {
        return new MinerAlgorithm
        {
            Name = _name,
            AlgorithmId = _algorithmId,
            MinerId = _minerId,
        };
    }
}

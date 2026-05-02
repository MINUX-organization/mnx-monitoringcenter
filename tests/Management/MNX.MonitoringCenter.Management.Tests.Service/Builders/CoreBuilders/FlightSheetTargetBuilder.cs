using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

using Miner = Core.Mining.Miner.Miner;

public class FlightSheetTargetBuilder
{
    protected Guid _id = Guid.NewGuid();
    protected BaseMiningConfig? _miningConfig = null;
    protected Guid _flightSheetId = Guid.NewGuid();
    protected Guid _minerId = Guid.NewGuid();
    protected Miner? _miner = null;

    internal FlightSheetTargetBuilder WithFlightSheetId(Guid flightSheetId)
    {
        _flightSheetId = flightSheetId;
        return this;
    }

    public FlightSheetTargetBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public FlightSheetTargetBuilder WithMiningConfig(Func<BaseMiningConfig> factory)
    {
        _miningConfig = factory();
        return this;
    }

    public FlightSheetTargetBuilder WithMiningConfig(BaseMiningConfig config)
    {
        _miningConfig = config;
        return this;
    }

    public FlightSheetTargetBuilder WithMiner(Func<MinerBuilder, MinerBuilder>? configure = null)
    {
        var builder = new MinerBuilder();
        builder = configure?.Invoke(builder) ?? builder;
        _miner = builder.Build();
        _minerId = _miner.Id;
        return this;
    }

    public FlightSheetTargetBuilder WithMiner(Miner miner)
    {
        _miner = miner;
        _minerId = miner.Id;
        return this;
    }

    public FlightSheetTarget Build()
    {
        return new FlightSheetTarget
        {
            Id = _id,
            MiningConfig = _miningConfig ??
                throw new ArgumentException("At least one MiningConfig is nessesary to initialize FlightSheetTarget"),
            FlightSheetId = _flightSheetId,
            MinerId = _minerId,
            Miner = _miner,
        };
    }
}

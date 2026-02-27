using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningConfigs;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

using Miner = Core.Mining.Miner.Miner;

public class FlightSheetTargetBuilder
{
    private Guid _id = Guid.NewGuid();
    private BaseMiningConfig? _miningConfig = null;
    private Guid _flightSheetId = Guid.NewGuid();
    private Guid _minerId = Guid.NewGuid();
    private Miner? _miner = null;

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

    public FlightSheetTargetBuilder WithFlightSheetId(Guid flightSheetId)
    {
        _flightSheetId = flightSheetId;
        return this;
    }

    public FlightSheetTargetBuilder WithMiner(Func<MinerBuilder, MinerBuilder> configure)
    {
        var builder = new MinerBuilder();
        builder = configure(builder);
        _miner = builder.Build();
        _minerId = _miner.Id;
        return this;
    }

    public FlightSheetTarget Build()
    {
        return new FlightSheetTarget
        {
            Id = _id,
            MiningConfig = _miningConfig ??
                throw new ArgumentException("It's nessesary to initialize mining config"),
            FlightSheetId = _flightSheetId,
            MinerId = _minerId,
            Miner = _miner,
        };
    }
}

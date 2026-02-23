using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.CoreBuilders.MiningConfigs;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.CoreBuilders;

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

    public FlightSheetTargetBuilder WithMiner(Action<MinerBuilder> configure)
    {
        var builder = new MinerBuilder();
        configure(builder);
        _miner = builder.Build();
        _minerId = _miner.Id;
        return this;
    }

    public FlightSheetTarget Build()
    {
        return new FlightSheetTarget
        {
            Id = _id,
            MiningConfig = _miningConfig ?? CreateDefaultMiningConfig(),
            FlightSheetId = _flightSheetId,
            MinerId = _minerId,
            Miner = _miner,
        };
    }

    private BaseMiningConfig CreateDefaultMiningConfig() => new GpuMiningConfigBuilder().Build();
}

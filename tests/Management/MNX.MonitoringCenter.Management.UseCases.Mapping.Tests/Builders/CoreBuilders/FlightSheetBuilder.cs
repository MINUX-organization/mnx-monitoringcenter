using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.CoreBuilders;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

public class FlightSheetBuilder
{
    private static int _counter = 1;

    private Guid _id = Guid.NewGuid();
    private string _name = $"FlightSheet_{_counter++}";
    private Guid _ownerId = Guid.NewGuid();
    private List<FlightSheetTarget> _targets = new(0);

    public FlightSheetBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public FlightSheetBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public FlightSheetBuilder WithOwnerId(Guid ownerId)
    {
        _ownerId = ownerId;
        return this;
    }

    public FlightSheetBuilder AddTarget(Action<FlightSheetTargetBuilder> configure)
    {
        var builder = new FlightSheetTargetBuilder();
        configure(builder);
        _targets.Add(builder.Build());
        return this;
    }

    public FlightSheetBuilder WithTargets(Func<List<FlightSheetTarget>> factory)
    {
        _targets.AddRange(factory());
        return this;
    }

    public FlightSheet Build()
    {
        return new FlightSheet
        {
            Id = _id,
            Name = _name,
            OwnerId = _ownerId,
            Targets = _targets,
        };
    }
}

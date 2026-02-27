using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;

public class FlightSheetInputModelBuilder
{
    private static int _counter = 1;

    private string _name = $"FlightSheetInputModelName_{_counter++}";
    private readonly List<FlightSheetTargetInputModel> _targets = new(0);

    public FlightSheetInputModelBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public FlightSheetInputModelBuilder AddFlightSheetTarget(
        Func<FlightSheetTargetInputModelBuilder, FlightSheetTargetInputModelBuilder> configure)
    {
        var builder = new FlightSheetTargetInputModelBuilder();
        builder = configure(builder);
        _targets.Add(builder.Build());
        return this;
    }

    public FlightSheetInputModelBuilder WithFlightSheetTargets(Func<List<FlightSheetTargetInputModel>> factory)
    {
        _targets.AddRange(factory());
        return this;
    }

    public FlightSheetInputModel Build()
    {
        return new FlightSheetInputModel
        {
            Name = _name,
            Targets = _targets,
        };
    }
}

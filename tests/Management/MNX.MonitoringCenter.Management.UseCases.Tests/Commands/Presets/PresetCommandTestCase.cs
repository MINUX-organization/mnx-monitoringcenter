using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets;

public static class PresetCommandTestCase
{
    public static IEnumerable<OverclockingInputModel> CreateCorrectOverclockingModel()
    {
        yield return new OverclockingInputModel(2000, 200, 1500, 50, 2000, 50, 1000, 0, 250, 90, 2000);
    }

    public static IEnumerable<OverclockingInputModel> CreateIncorrectOverclockingModel()
    {
        yield return new OverclockingInputModel(500, 50, 500, -100, 500, -75, 800, -100, 60, 50, 100);
    }
}

using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets;

public static class PresetCommandTestCase
{
    public static IEnumerable<PresetModel> CreateCorrectPresetModel()
    {
        yield return new PresetModel(memoryClock: 1313,
                                     coreClock: 2235,
                                     powerLimit: 150,
                                     criticalTemperature: 105,
                                     fanSpeed: 99);
    }

    public static IEnumerable<PresetModel> CreateIncorrectPresetModel()
    {
        yield return new PresetModel(memoryClock: -1,
                                     coreClock: -1,
                                     powerLimit: -1,
                                     criticalTemperature: -1,
                                     fanSpeed: -1);
    }
}

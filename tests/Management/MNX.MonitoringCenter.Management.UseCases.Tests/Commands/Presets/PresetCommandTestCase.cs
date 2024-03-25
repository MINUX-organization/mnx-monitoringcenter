using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets;

public static class PresetCommandTestCase
{
    public static IEnumerable<SavePresetInputModel> CreateCorrectSavePresetModel()
    {
        var presetInputModel = new PresetInputModel(memoryClock: 1313,
                                     coreClock: 2235,
                                     powerLimit: 150,
                                     criticalTemperature: 105,
                                     fanSpeed: 99);

        yield return new SavePresetInputModel("GeForce RTX 4090", presetInputModel);
    }

    public static IEnumerable<SavePresetInputModel> CreateIncorrectSavePresetModel()
    {
        var presetInputModel = new PresetInputModel(memoryClock: -1,
                                     coreClock: -1,
                                     powerLimit: -1,
                                     criticalTemperature: -1,
                                     fanSpeed: -1);

        yield return new SavePresetInputModel("GeForce GTX 4090", presetInputModel);
    }

    public static IEnumerable<PresetInputModel> CreateCorrectPresetModel()
    {
        yield return new PresetInputModel(memoryClock: 1313,
                                     coreClock: 2235,
                                     powerLimit: 150,
                                     criticalTemperature: 105,
                                     fanSpeed: 99);
    }

    public static IEnumerable<PresetInputModel> CreateIncorrectPresetModel()
    {
        yield return new PresetInputModel(memoryClock: -1,
                                     coreClock: -1,
                                     powerLimit: -1,
                                     criticalTemperature: -1,
                                     fanSpeed: -1);
    }
}

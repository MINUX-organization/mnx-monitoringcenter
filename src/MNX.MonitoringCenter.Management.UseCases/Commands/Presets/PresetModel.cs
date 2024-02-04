namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets;

/// <summary>
/// Модель пресета
/// </summary>
public class PresetModel
{
    /// <summary>
    /// Тактовая чатсота памяти в мегагерцах
    /// </summary>
    public int MemoryClock { get; }

    /// <summary>
    /// Тактовая частота ядра в мегагерцах
    /// </summary>
    public int CoreClock { get; }

    /// <summary>
    /// Ограничение мощности в ваттах
    /// </summary>
    public int PowerLimit { get; }

    /// <summary>
    /// Критическая температура в градусах Цельсия
    /// </summary>
    public int CriticalTemperature { get; }

    /// <summary>
    /// Скорость вентилятора в процентах
    /// </summary>
    public int FanSpeed { get; }

    public PresetModel(int memoryClock, int coreClock, int powerLimit, int criticalTemperature, int fanSpeed)
    {
        MemoryClock = memoryClock;
        CoreClock = coreClock;
        PowerLimit = powerLimit;
        CriticalTemperature = criticalTemperature;
        FanSpeed = fanSpeed;
    }
}

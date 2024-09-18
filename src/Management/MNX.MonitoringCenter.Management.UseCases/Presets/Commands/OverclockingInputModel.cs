namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets;

/// <summary>
/// Модель разгона
/// </summary>
public class OverclockingInputModel
{
    /// <summary>
    /// Фиксированная частота ядра
    /// </summary>
    public int CoreClockLock { get; }

    /// <summary>
    /// Смещение частоты ядра
    /// </summary>
    public int CoreClockOffset { get; }

    /// <summary>
    /// Фиксированная частота памяти
    /// </summary>
    public int MemoryClockLock { get; }

    /// <summary>
    /// Смещение частоты памяти
    /// </summary>
    public int MemoryClockOffset { get; }

    /// <summary>
    /// Напряжение на ядре
    /// </summary>
    public int CoreVoltage { get; }

    /// <summary>
    /// Смещение напряжения на ядре
    /// </summary>
    public int CoreVoltageOffset { get; }

    /// <summary>
    /// Напряжение памяти
    /// </summary>
    public int MemoryVoltage { get; }

    /// <summary>
    /// Смещение напряжения памяти
    /// </summary>
    public int MemoryVoltageOffset { get; }

    /// <summary>
    /// Ограничение мощности
    /// </summary>
    public int PowerLimit { get; }

    /// <summary>
    /// Скорость вентилятора
    /// </summary>
    public int FanSpeed { get; }

    public OverclockingInputModel(int coreClockLock,
                                  int coreClockOffset,
                                  int memoryClockLock,
                                  int memoryClockOffset,
                                  int coreVoltage,
                                  int coreVoltageOffset,
                                  int memoryVoltage,
                                  int memoryVoltageOffset,
                                  int powerLimit,
                                  int fanSpeed)
    {
        CoreClockLock = coreClockLock;
        CoreClockOffset = coreClockOffset;
        MemoryClockLock = memoryClockLock;
        MemoryClockOffset = memoryClockOffset;
        CoreVoltage = coreVoltage;
        CoreVoltageOffset = coreVoltageOffset;
        MemoryVoltage = memoryVoltage;
        MemoryVoltageOffset = memoryVoltageOffset;
        PowerLimit = powerLimit;
        FanSpeed = fanSpeed;
    }
}

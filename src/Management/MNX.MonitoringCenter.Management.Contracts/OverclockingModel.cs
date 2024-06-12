namespace MNX.MonitoringCenter.Management.Contracts;

/// <summary>
/// Модель для разгона
/// </summary>
public class OverclockingModel
{ 
    /// <summary>
    /// Фиксированная частота ядра
    /// </summary>
    public int CoreClockLock { get; set; }

    /// <summary>
    /// Смещение частоты ядра
    /// </summary>
    public int CoreClockOffset { get; set; }

    /// <summary>
    /// Фиксированная частота памяти
    /// </summary>
    public int MemoryClockLock { get; set; }

    /// <summary>
    /// Смещение частоты памяти
    /// </summary>
    public int MemoryClockOffset { get; set; }

    /// <summary>
    /// Напряжение на ядре
    /// </summary>
    public int CoreVoltage { get; set; }

    /// <summary>
    /// Смещение напряжения на ядре
    /// </summary>
    public int CoreVoltageOffset { get; set; }

    /// <summary>
    /// Напряжение памяти
    /// </summary>
    public int MemoryVoltage { get; set; }

    /// <summary>
    /// Смещение напряжения памяти
    /// </summary>
    public int MemoryVoltageOffset { get; set; }

    /// <summary>
    /// Ограничение мощности
    /// </summary>
    public int PowerLimit { get; set; }

    /// <summary>
    /// Критическая температура
    /// </summary>
    public int CriticalTemperature { get; set; }

    /// <summary>
    /// Скорость вентилятора
    /// </summary>
    public int FanSpeed { get; set; }
}

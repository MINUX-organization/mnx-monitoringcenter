using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.DataAccess.Overclocking;

/// <summary>
/// Разгон.
/// </summary>
public class OverclockingDto
{
    #region General

    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Тип целевого майнинг устройства.
    /// </summary>
    public OverclockingTargetDeviceType TargetDeviceType { get; init; }

    /// <summary>
    /// Ограничение мощности.
    /// </summary>
    public int? PowerLimit { get; set; }

    /// <summary>
    /// Скорость вентилятора.
    /// </summary>
    public int? FanSpeed { get; set; }

    /// <summary>
    /// Фиксированная частота ядра.
    /// </summary>
    public int? CoreClockLock { get; set; }

    /// <summary>
    /// Напряжение на ядре.
    /// </summary>
    public int? CoreVoltage { get; set; }

    /// <summary>
    /// Смещение напряжения на ядре.
    /// </summary>
    public int? CoreVoltageOffset { get; set; }

    /// <summary>
    /// Напряжение памяти.
    /// </summary>
    public int? MemoryVoltage { get; set; }

    /// <summary>
    /// Фиксированная частота памяти.
    /// </summary>
    public int? MemoryClockLock { get; set; }

    #endregion

    #region Nvidia

    /// <summary>
    /// Смещение частоты ядра.
    /// </summary>
    public int? CoreClockOffset { get; set; }

    /// <summary>
    /// Смещение частоты памяти.
    /// </summary>
    public int? MemoryClockOffset { get; set; }

    /// <summary>
    /// Смещение напряжения памяти.
    /// </summary>
    public int? MemoryVoltageOffset { get; set; }

    #endregion

    #region Amd

    /// <summary>
    /// Уровни частоты ядра.
    /// </summary>
    public int? CoreClockState { get; set; }

    /// <summary>
    /// P-состояния памяти: частотные режимы в зависимости от нагрузки.
    /// </summary>
    public int? MemoryClockState { get; set; }

    /// <summary>
    /// Напряжение контроллера памяти.
    /// </summary>
    public int? MemoryControllerVoltage { get; set; }

    /// <summary>
    /// Частота SoC (системной части GPU).
    /// </summary>
    public int? SocFrequency { get; set; }

    /// <summary>
    /// Напряжение SoC.
    /// </summary>
    public int? SocVoltage { get; set; }

    /// <summary>
    /// Настройки памяти.
    /// </summary>
    public string? MemoryTweak { get; set; }

    /// <summary>
    /// Автоматический разгон.
    /// </summary>
    public bool? EnhancedOverclock { get; set; }

    /// <summary>
    /// Альтернативное снижение напряжения.
    /// </summary>
    public bool? AlternativeDownVoltage { get; set; }

    #endregion
}

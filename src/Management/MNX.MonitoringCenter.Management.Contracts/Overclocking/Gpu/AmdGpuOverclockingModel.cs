using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;

/// <summary>
/// Модель разгона для видеокарты Amd.
/// </summary>
public record AmdGpuOverclockingModel : IOverclockingModel
{
    private OverclockingTargetDeviceType _targetDeviceType = OverclockingTargetDeviceType.AmdGPU;

    /// <<inheritdoc/>
    [JsonPropertyName("$type")]
    public OverclockingTargetDeviceType TargetDeviceType
    {
        get => _targetDeviceType;
        init => _targetDeviceType = OverclockingTargetDeviceType.AmdGPU;
    }

    /// <summary>
    /// Разгон вентилятора.
    /// </summary>
    public required IFanOverclockingModel FanOverclocking { get; set; }

    /// <summary>
    /// Ограничение мощности
    /// </summary>
    public int PowerLimit { get; set; }

    /// <summary>
    /// Блокировка частоты ядра.
    /// </summary>
    public int CoreClockLock { get; set; }

    /// <summary>
    /// Уровни частоты ядра.
    /// </summary>
    public int CoreClockState { get; set; }

    /// <summary>
    /// Напряжение на ядро GPU.
    /// </summary>
    public int CoreVoltage { get; set; }

    /// <summary>
    /// Смещение напряжения ядра.
    /// </summary>
    public int CoreVoltageOffset { get; set; }

    /// <summary>
    /// Частота видеопамяти.
    /// </summary>
    public int MemoryClockLock { get; set; }

    /// <summary>
    /// P-состояния памяти: частотные режимы в зависимости от нагрузки.
    /// </summary>
    public int MemoryClockState { get; set; }

    /// <summary>
    /// Напряжение видеопамяти.
    /// </summary>
    public int MemoryVoltage { get; set; }

    /// <summary>
    /// Напряжение контроллера памяти.
    /// </summary>
    public int MemoryControllerVoltage { get; set; }

    /// <summary>
    /// Настройки памяти.
    /// </summary>
    public string? MemoryTweak { get; set; }

    /// <summary>
    /// Автоматический разгон.
    /// </summary>
    public bool EnhancedOverclock { get; set; }

    /// <summary>
    /// Альтернативное снижение напряжения.
    /// </summary>
    public bool AlternativeDownVoltage { get; set; }

    /// <summary>
    /// Частота SoC (системной части GPU).
    /// </summary>
    public int SocFrequency { get; set; }

    /// <summary>
    /// Напряжение SoC.
    /// </summary>
    public int SocVoltage { get; set; }
}

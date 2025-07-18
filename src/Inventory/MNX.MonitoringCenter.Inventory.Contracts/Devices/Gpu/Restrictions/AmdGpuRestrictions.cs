namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

/// <summary>
/// Ограничения видеокарты модели Amd
/// </summary>
public record AmdGpuRestrictions : GpuRestrictions
{
    /// <inheritdoc/>
    public override TargetGpuType TargetGpuType
    {
        get => TargetGpuType.Amd;
    }

    /// <summary>
    /// Ограничения фиксации частоты ядра. 
    /// </summary>
    public required IntegerTypeRestrictions ClockCoreLock { get; init; }

    /// <summary>
    /// Ограничения уровней частоты ядра.
    /// </summary>
    public required IntegerTypeRestrictions ClockCoreState { get; init; }

    /// <summary>
    /// Ограничения частоты видеопамяти.
    /// </summary>
    public required IntegerTypeRestrictions ClockMemoryLock { get; init; }

    /// <summary>
    /// Ограничения P-states частоты видеопамяти.
    /// </summary>
    public required IntegerTypeRestrictions ClockMemoryState { get; init; }

    /// <summary>
    /// Ограничения фиксации напряжения процессора.
    /// </summary>
    public required IntegerTypeRestrictions VoltageCoreLock { get; init; }

    /// <summary>
    /// Ограничения смещения напряжения процессора.
    /// </summary>
    public required IntegerTypeRestrictions VoltageCoreOffset { get; init; }

    /// <summary>
    /// Ограничения фиксации напряжения видеопамяти.
    /// </summary>
    public required IntegerTypeRestrictions VoltageMemoryLock { get; init; }

    /// <summary>
    /// Ограничения напряжения контроллера памяти.
    /// </summary>
    public required IntegerTypeRestrictions VoltageMemoryController { get; init; }

    /// <summary>
    /// Ограничения частоты Soc (системной части GPU).
    /// </summary>
    public required IntegerTypeRestrictions SocFrequency { get; init; }

    /// <summary>
    /// Ограничения напряжения Soc (системной части GPU).
    /// </summary>
    public required IntegerTypeRestrictions SocVoltage { get; init; }
}

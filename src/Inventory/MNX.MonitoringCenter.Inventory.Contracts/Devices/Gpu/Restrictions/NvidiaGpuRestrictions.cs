namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

/// <summary>
/// Ограничения видеокарты модели Nvidia.
/// </summary>
public record NvidiaGpuRestrictions : GpuRestrictions
{
    /// <inheritdoc/>
    public override TargetGpuType TargetGpuType
    {
        get => TargetGpuType.Nvidia;
    }

    /// <summary>
    /// Ограничения фиксации частоты процессора.
    /// </summary>
    public required IntegerTypeRestrictions ClockCoreLock { get; init; }

    /// <summary>
    /// Ограничения смещения частоты процессора.
    /// </summary>
    public required IntegerTypeRestrictions ClockCoreOffset { get; init; }

    /// <summary>
    /// Ограничения фиксации частоты видеопамяти.
    /// </summary>
    public required IntegerTypeRestrictions ClockMemoryLock { get; init; }

    /// <summary>
    /// Ограничения смещения частоты видеопамяти.
    /// </summary>
    public required IntegerTypeRestrictions ClockMemoryOffset { get; init; }

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
    /// Ограничения смещения напряжения видеопамяти.
    /// </summary>
    public required IntegerTypeRestrictions VoltageMemoryOffset { get; init; }
}

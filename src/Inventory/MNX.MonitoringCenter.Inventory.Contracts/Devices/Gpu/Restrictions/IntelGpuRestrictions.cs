namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

/// <summary>
/// Ограничения видеокарт модели Intel.
/// </summary>
/// <remarks>
/// На данный момент не реализуется,
/// так как нет поддержки видеокарт данной модели.
/// </remarks>
public record IntelGpuRestrictions : GpuRestrictions
{
    /// <inheritdoc/>
    public override TargetGpuType TargetGpuType
    {
        get => TargetGpuType.Intel;
    }
}

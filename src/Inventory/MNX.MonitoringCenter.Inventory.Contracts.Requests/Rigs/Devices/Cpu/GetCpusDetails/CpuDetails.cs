namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu.GetCpusDetails;

/// <summary>
/// Детали процессора.
/// </summary>
public record CpuDetails : Contracts.Devices.Cpu.Cpu
{
    /// <summary>
    /// Название рига.
    /// </summary>
    public required string RigName { get; init; }
}

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu.GetCpusInfo;

/// <summary>
/// Модель процессора.
/// </summary>
public record CpuModel : Contracts.Devices.Cpu.Cpu
{
    /// <summary>
    /// Название рига.
    /// </summary>
    public required string RigName { get; init; }
}

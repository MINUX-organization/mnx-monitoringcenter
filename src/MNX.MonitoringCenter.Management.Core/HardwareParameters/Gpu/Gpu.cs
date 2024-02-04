namespace MNX.MonitoringCenter.Management.Core.HardwareParameters.Gpu;

public class Gpu
{
    public string Id { get; set; }

    public string Name { get; set; }

    public GpuClocks Clocks { get; set; }

    public GpuInfo Information { get; set; }

    public int TotalMemory { get; set; }

    public GpuPower Power { get; set; }

    public GpuTemperature Temperature { get; set; }
}
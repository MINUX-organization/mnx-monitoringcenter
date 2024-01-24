namespace MINUX.Backend.Worker.Core.HardwareParameters.Cpu;

public class Cpu
{
    public string Id { get; set; }

    public CpuClocks Clocks { get; set; }

    public CpuInfo Inforamtion { get; set; }
}
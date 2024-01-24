namespace MINUX.Backend.Worker.Core.HardwareParameters.Gpu;

public class GpuPower
{
    public int DefaultLimit { get; set; }

    public int EnforcedLimit { get; set; }

    public int Maximum { get; set; }

    public int Minimal {  get; set; }
}
namespace MINUX.Backend.Worker.Core.StaticData.Cpu;

public class CpuInfo
{
    public string Architecture { get; set; }

    public CpuCache Cache { get; set; }

    public CpuCores Cores { get; set; }

    public string Manufacturer { get; set; }

    public string ModelName { get; set; }

    public string OpModes { get; set; }
}
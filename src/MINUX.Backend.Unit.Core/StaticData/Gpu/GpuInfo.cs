namespace MINUX.Backend.Unit.Core.StaticData.Gpu;

public class GpuInfo
{
    public string DriverVersion { get; set; }

    public string Manufacturer { get; set; }

    public GpuPci Pci { get; set; }

    public string Perephery { get; set; }

    public string SerialNumber { get; set; }

    public GpuTechnology Technology { get; set; }
}
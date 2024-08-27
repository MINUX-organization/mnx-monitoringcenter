using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts;

public class InventoryModel
{
    public List<Cpu.Cpu> Cpus { get; set; } = new();

    public List<Gpu.Gpu> Gpus { get; set; } = new();

    public List<Drive.Drive> Drives { get; set; } = new();

    public List<InternetAdapter.InternetAdapter> InternetAdapters { get; set; } = new();

    public Motherboard.Motherboard Motherboard { get; set; } = new();

    public SoftwareInventory Software { get; set; }
}

using MNX.MonitoringCenter.Inventory.Contracts;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

internal class Inventory
{
    public long Id { get; set; }

    public Guid RigId { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public DateTimeOffset? EndDate { get; set; }

    public List<Contracts.Cpu.Cpu> Cpus { get; set; } = new();

    public List<Contracts.Gpu.Gpu> Gpus { get; set; } = new();

    public List<Contracts.Drive.Drive> Drives { get; set; } = new();

    public List<Contracts.InternetAdapter.InternetAdapter> InternetAdapters { get; set; } = new();

    public Contracts.Motherboard.Motherboard Motherboard { get; set; } = new();

    public SoftwareInventory Software { get; set; }
}

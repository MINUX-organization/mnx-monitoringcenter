namespace MNX.MonitoringCenter.Inventory.Contracts;

public class InventoryMsg
{
    public Guid RigId { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public InventoryModel Inventory { get; set; }
}

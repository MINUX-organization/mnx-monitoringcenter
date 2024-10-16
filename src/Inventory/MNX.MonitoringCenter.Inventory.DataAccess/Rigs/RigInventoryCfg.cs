using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Motherboard;

namespace MNX.MonitoringCenter.Inventory.DataAccess.RigInventory;

/// <summary>
/// Конфигурация для таблицы с инвентаризацией ригов.
/// </summary>
internal class RigInventoryCfg : IEntityTypeConfiguration<RigInventory>
{
    public void Configure(EntityTypeBuilder<RigInventory> builder)
    {
        builder.HasOne(x => x.Motherboard)
               .WithOne()
               .HasForeignKey<Motherboard>("InventoryId");

        builder.HasOne(x => x.Software)
               .WithOne()
               .HasForeignKey<SoftwareInventory>("InventoryId");

        builder.HasIndex(x => new { x.RigId, x.CreatedDateTime }).IsDescending(false, true);
    }
}

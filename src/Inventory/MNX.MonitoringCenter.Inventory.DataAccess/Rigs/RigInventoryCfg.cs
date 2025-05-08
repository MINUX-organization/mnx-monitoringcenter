using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Motherboard;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Software;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs;

/// <summary>
/// Конфигурация для таблицы с инвентаризацией ригов.
/// </summary>
internal class RigInventoryCfg : IEntityTypeConfiguration<RigInventory>
{
    public void Configure(EntityTypeBuilder<RigInventory> builder)
    {
        builder.HasOne(x => x.Motherboard)
               .WithOne()
               .HasForeignKey<Motherboard>("RigInventoryId");

        builder.HasOne(x => x.Software)
               .WithOne()
               .HasForeignKey<SoftwareInventoryDto>("RigInventoryId");

        builder.HasOne(x => x.Rig)
               .WithMany(x => x.Inventories)
               .HasForeignKey(x => x.RigId);

        builder.HasIndex(x => new { x.RigId, x.CreatedDateTime }).IsDescending(false, true);
    }
}

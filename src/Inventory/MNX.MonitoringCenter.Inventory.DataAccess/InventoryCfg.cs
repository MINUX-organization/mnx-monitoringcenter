using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Inventory.Contracts;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Конфигурация для таблицы с инвентаризацией.
/// </summary>
internal class InventoryCfg : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.HasOne(x => x.Motherboard)
               .WithOne()
               .HasForeignKey<Contracts.Motherboard.Motherboard>("InventoryId");

        builder.HasOne(x => x.Software)
               .WithOne()
               .HasForeignKey<SoftwareInventory>("InventoryId");

        builder.HasIndex(x => new { x.RigId, x.CreatedDate }).IsDescending(false, true);
    }
}

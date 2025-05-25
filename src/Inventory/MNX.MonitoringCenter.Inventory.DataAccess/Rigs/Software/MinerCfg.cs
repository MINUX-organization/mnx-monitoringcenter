using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Software;

/// <summary>
/// Конфигурация для таблицы с инвентаризацией майнеров.
/// </summary>
internal class MinerCfg : IEntityTypeConfiguration<MinerInventory>
{
    public void Configure(EntityTypeBuilder<MinerInventory> builder)
    {
        builder.HasOne<SoftwareInventory>()
            .WithMany(x => x.Miners)
            .HasForeignKey("rig_inventory_id")
            .HasPrincipalKey("RigInventoryId");

        builder.HasIndex(miner => miner.Name);
    }
}

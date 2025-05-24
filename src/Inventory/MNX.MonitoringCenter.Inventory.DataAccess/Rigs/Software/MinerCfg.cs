using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Software;

/// <summary>
/// Конфигурация для таблицы с инвентаризацией майнеров.
/// </summary>
internal class MinerCfg : IEntityTypeConfiguration<Contracts.MinerInventory>
{
    public void Configure(EntityTypeBuilder<Contracts.MinerInventory> builder)
    {
        builder.HasOne<SoftwareInventoryDto>()
            .WithMany(x => x.Miners)
            .HasForeignKey("software_id", "rig_inventory_id")
            .HasPrincipalKey("Id", "RigInventoryId");

        builder.HasIndex(miner => miner.Name);
    }
}

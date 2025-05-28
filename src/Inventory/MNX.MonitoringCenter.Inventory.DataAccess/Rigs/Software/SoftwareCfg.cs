using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Software;

/// <summary>
/// Конфигурация для таблицы с инвентаризацией программного обеспечения.
/// </summary>
internal class SoftwareCfg : IEntityTypeConfiguration<SoftwareInventory>
{
    public void Configure(EntityTypeBuilder<SoftwareInventory> builder)
    {
        builder.HasKey("RigInventoryId");
    }
}

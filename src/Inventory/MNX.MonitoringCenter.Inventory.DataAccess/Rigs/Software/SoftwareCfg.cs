using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Software;

/// <summary>
/// Конфигурация для таблицы с инвентаризацией программного обеспечения.
/// </summary>
internal class SoftwareCfg : IEntityTypeConfiguration<SoftwareInventoryDto>
{
    public void Configure(EntityTypeBuilder<SoftwareInventoryDto> builder)
    {
        builder.HasKey("RigInventoryId", "Id");
    }
}

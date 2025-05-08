using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Software;

/// <summary>
/// Конфигурация для таблицы с инвентаризацией программного обеспечения.
/// </summary>
internal class SoftwareCfg : IEntityTypeConfiguration<SoftwareInventoryDto>
{
    public void Configure(EntityTypeBuilder<SoftwareInventoryDto> builder)
    {
        builder.HasKey("RigInventoryId", "Id");

        builder.Property(x => x.Miners)
               .HasConversion(
                    x => JsonSerializer.Serialize(x, (JsonSerializerOptions)null!),
                    x => JsonSerializer.Deserialize<Dictionary<string, string>>(x, (JsonSerializerOptions)null!)!);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Inventory.Contracts;
using System.Text.Json;

namespace MNX.MonitoringCenter.Inventory.DataAccess.RigInventory.Software;

/// <summary>
/// Конфигурация для таблицы с инвентаризацией программного обеспечения.
/// </summary>
internal class SoftwareCfg : IEntityTypeConfiguration<SoftwareInventory>
{
    public void Configure(EntityTypeBuilder<SoftwareInventory> builder)
    {
        builder.Property(x => x.Miners)
               .HasConversion(
                    x => JsonSerializer.Serialize(x, (JsonSerializerOptions)null!),
                    x => JsonSerializer.Deserialize<Dictionary<string, string>>(x, (JsonSerializerOptions)null!)!);
    }
}

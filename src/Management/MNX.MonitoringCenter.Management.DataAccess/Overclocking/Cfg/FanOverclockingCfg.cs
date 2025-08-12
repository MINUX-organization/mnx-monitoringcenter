using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using System.Text.Json;

namespace MNX.MonitoringCenter.Management.DataAccess.Overclocking.Cfg;

/// <summary>
/// Конфигурация для таблицы сущности <see cref="FanOverclockingDto"/>.
/// </summary>
public class FanOverclockingCfg : IEntityTypeConfiguration<FanOverclockingDto>
{
    ///
    public void Configure(EntityTypeBuilder<FanOverclockingDto> builder)
    {
        builder.ToTable("fan_overclocking");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<FanOverclockingType>(v));

        builder.Property(x => x.TargetPoints)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<FanGraphicPoint[]>(v, (JsonSerializerOptions?)null)!);
    }
}

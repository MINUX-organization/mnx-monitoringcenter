using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.Overclocking;

/// <summary>
/// Конфигурация для таблицы с разгоном.
/// </summary>
public class OverclockingCfg : IEntityTypeConfiguration<OverclockingDto>
{
    public void Configure(EntityTypeBuilder<OverclockingDto> builder)
    {
        builder.Property(x => x.TargetDeviceType).HasConversion<string>();
    }
}

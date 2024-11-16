using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core.MiningDevice;

namespace MNX.MonitoringCenter.Management.DataAccess.MiningDevice;

/// <summary>
/// Конфигурация для таблицы с майниг устройствами.
/// </summary>
internal class MiningDeviceCfg : IEntityTypeConfiguration<MiningDeviceInfo>
{
    public void Configure(EntityTypeBuilder<MiningDeviceInfo> builder)
    {
        builder.HasIndex(x => x.RigId);
        builder.HasIndex(x => x.OwnerId);

        builder.HasQueryFilter(x => x.IsActive);

        builder.Property(x => x.Type)
               .HasConversion<string>();

        builder.Ignore(x => x.FlightSheet);
    }
}

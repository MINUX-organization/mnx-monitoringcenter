using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking;

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

        builder.HasOne<Core.Overclocking.Preset>()
               .WithMany()
               .HasForeignKey(x => x.PresetId)
               .IsRequired(false);

        builder.HasQueryFilter(x => x.LifeCycleStatus != MiningDeviceLifeCycleStatus.Inactive);

        builder.Property(x => x.Type).HasConversion<string>();
        builder.Property(x => x.LifeCycleStatus).HasConversion<string>();

        builder.Ignore(x => x.FlightSheet);
        builder.Ignore(x => x.Preset);
    }
}

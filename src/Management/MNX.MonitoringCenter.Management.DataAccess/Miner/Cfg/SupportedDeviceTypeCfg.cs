using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core.Miner;

namespace MNX.MonitoringCenter.Management.DataAccess.Miner.Cfg;

/// <summary>
/// Конфигурация таблицы с поддерживаемыми майнером девайсами. 
/// </summary>
internal class SupportedDeviceTypeCfg : IEntityTypeConfiguration<SupportedDeviceType>
{
    public void Configure(EntityTypeBuilder<SupportedDeviceType> builder)
    {
        builder.Property(x => x.DeviceType)
            .HasConversion<string>();

        builder.HasKey(x => new { x.MinerId, x.DeviceType });

        builder.HasOne<Core.Miner.Miner>()
            .WithMany(miner => miner.DeviceTypes)
            .HasForeignKey(supportedDevice => supportedDevice.MinerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
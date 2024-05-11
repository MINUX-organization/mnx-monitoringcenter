using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto.Devices;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Cfg;

/// <summary>
/// Конфигурация таблицы с майнинг устройствами.
/// </summary>
internal class MiningDeviceCfg : IEntityTypeConfiguration<MiningDeviceDto>
{
    public void Configure(EntityTypeBuilder<MiningDeviceDto> builder)
    {
        builder.HasDiscriminator(x => x.Type)
               .HasValue<CpuDto>(MiningDeviceType.CPU)
               .HasValue<GpuDto>(MiningDeviceType.GPU)
               .HasValue<HddDto>(MiningDeviceType.HDD);
    }
}

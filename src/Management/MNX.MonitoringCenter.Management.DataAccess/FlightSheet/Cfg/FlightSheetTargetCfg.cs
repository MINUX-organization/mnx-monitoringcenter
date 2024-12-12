using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto.Target;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Cfg;

/// <summary>
/// Конфигурация таблицы с таргетами полётных листов.
/// </summary>
internal class FlightSheetTargetCfg : IEntityTypeConfiguration<BaseFlightSheetTargetDto>
{
    public void Configure(EntityTypeBuilder<BaseFlightSheetTargetDto> builder)
    {
        builder.HasDiscriminator(x => x.DeviceType)
               .HasValue<CpuFlightSheetTargetDto>(MiningDeviceType.CPU)
               .HasValue<GpuFlightSheetTargetDto>(MiningDeviceType.GPU);

        builder.HasOne<FlightSheetDto>()
               .WithMany(flightSheet => flightSheet.Targets)
               .HasForeignKey(target => target.FlightSheetId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
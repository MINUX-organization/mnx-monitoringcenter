using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Cfg;

/// <summary>
/// Конфигурация таблицы с таргетами полётных листов.
/// </summary>
internal class FlightSheetTargetCfg : IEntityTypeConfiguration<FlightSheetTargetBase>
{
    public void Configure(EntityTypeBuilder<FlightSheetTargetBase> builder)
    {
        builder.HasDiscriminator(x => x.Type)
               .HasValue<CpuFlightSheetTarget>(FlightSheetTargetType.CPU)
               .HasValue<GpuFlightSheetTarget>(FlightSheetTargetType.GPU);

        builder.HasOne<Core.FlightSheet.FlightSheet>()
               .WithMany(flightSheet => flightSheet.Targets)
               .HasForeignKey(target => target.FlightSheetId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
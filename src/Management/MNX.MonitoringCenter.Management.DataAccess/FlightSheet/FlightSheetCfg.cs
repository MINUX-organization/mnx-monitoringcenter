using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core.FlightSheet;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet;

/// <summary>
/// Конфигурация таблицы с полётными листами.
/// </summary>
internal class FlightSheetCfg : IEntityTypeConfiguration<FlightSheetBase>
{
    public void Configure(EntityTypeBuilder<FlightSheetBase> builder)
    {
        builder.HasIndex(x => x.UserId);

        builder.HasDiscriminator(x => x.Type)
               .HasValue<CpuFlightSheet>(FlightSheetType.CPU)
               .HasValue<GpuFlightSheet>(FlightSheetType.GPU);

        builder.HasOne<Core.Miner>()
               .WithMany()
               .HasForeignKey(x => x.Miner);
    }
}
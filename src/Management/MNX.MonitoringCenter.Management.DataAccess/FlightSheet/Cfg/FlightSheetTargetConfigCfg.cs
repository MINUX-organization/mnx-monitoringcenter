using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Cfg;

/// <summary>
/// Таблица для конфигов таргетов полётного листа.
/// </summary>
internal class FlightSheetTargetConfigCfg : IEntityTypeConfiguration<FlightSheetTargetConfig>
{
    public void Configure(EntityTypeBuilder<FlightSheetTargetConfig> builder)
    {
        builder.HasOne<FlightSheetTargetBase>()
               .WithMany(x => x.Configs)
               .HasForeignKey("flight_sheet_target_base_id")
               .OnDelete(DeleteBehavior.Cascade);
    }
}

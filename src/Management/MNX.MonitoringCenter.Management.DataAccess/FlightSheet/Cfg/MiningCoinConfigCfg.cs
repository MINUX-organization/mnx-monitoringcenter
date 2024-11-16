using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core.Miner.Configs;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto.Target;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Cfg;

/// <summary>
/// Таблица для конфигов таргетов полётного листа.
/// </summary>
internal class MiningCoinConfigCfg : IEntityTypeConfiguration<MiningCoinConfig>
{
    public void Configure(EntityTypeBuilder<MiningCoinConfig> builder)
    {
        builder.HasOne<BaseFlightSheetTargetDto>()
               .WithMany(x => x.CoinConfigs)
               .HasForeignKey("flight_sheet_target_id")
               .OnDelete(DeleteBehavior.Cascade);
    }
}

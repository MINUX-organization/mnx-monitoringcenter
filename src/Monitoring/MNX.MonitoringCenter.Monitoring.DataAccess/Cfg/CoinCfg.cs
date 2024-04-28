using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Monitoring.Core;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Cfg;

/// <summary>
/// Конфигурация таблицы с монетами.
/// </summary>
internal class CoinCfg : IEntityTypeConfiguration<Coin>
{
    public void Configure(EntityTypeBuilder<Coin> builder)
    {
        builder.HasMany<FlightSheet>()
               .WithMany(flightSheet => flightSheet.Coins);
    }
}

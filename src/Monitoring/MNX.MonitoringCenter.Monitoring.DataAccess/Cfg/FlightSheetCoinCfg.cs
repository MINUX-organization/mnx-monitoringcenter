using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Cfg;

/// <summary>
/// Конфигурация для таблицы с монетами полётного листа.
/// </summary>
internal class FlightSheetCoinCfg : IEntityTypeConfiguration<FlightSheetCoinDto>
{
    public void Configure(EntityTypeBuilder<FlightSheetCoinDto> builder)
    {
        builder.HasKey(x => new { x.CoinId, x.FlightSheetId });
    }
}

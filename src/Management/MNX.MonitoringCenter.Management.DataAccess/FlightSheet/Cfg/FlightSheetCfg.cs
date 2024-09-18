using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Cfg;

/// <summary>
/// Конфигурация для таблицы с полётными листами.
/// </summary>
internal class FlightSheetCfg : IEntityTypeConfiguration<Core.FlightSheet.FlightSheet>
{
    public void Configure(EntityTypeBuilder<Core.FlightSheet.FlightSheet> builder)
    {
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Name);
    }
}

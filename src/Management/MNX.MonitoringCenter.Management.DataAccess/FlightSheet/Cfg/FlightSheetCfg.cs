using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Cfg;

/// <summary>
/// Конфигурация для таблицы с полётными листами.
/// </summary>
internal class FlightSheetCfg : IEntityTypeConfiguration<FlightSheetDto>
{
    public void Configure(EntityTypeBuilder<FlightSheetDto> builder)
    {
        builder.HasIndex(x => x.OwnerId);
        builder.HasIndex(x => x.Name);
    }
}

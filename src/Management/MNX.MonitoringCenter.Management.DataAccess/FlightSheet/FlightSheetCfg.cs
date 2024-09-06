using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet;

internal class FlightSheetCfg : IEntityTypeConfiguration<Core.FlightSheet>
{
    public void Configure(EntityTypeBuilder<Core.FlightSheet> builder)
    {
        builder.HasIndex(x => x.UserId);
    }
}
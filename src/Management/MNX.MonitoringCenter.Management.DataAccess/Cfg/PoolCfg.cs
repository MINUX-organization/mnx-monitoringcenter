using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.DataAccess.Cfg;

internal class PoolCfg : IEntityTypeConfiguration<Pool>
{
    public void Configure(EntityTypeBuilder<Pool> builder)
    {
        builder.HasOne<Cryptocurrency>()
               .WithMany()
               .HasForeignKey(x => x.Cryptocurrency);
    }
}

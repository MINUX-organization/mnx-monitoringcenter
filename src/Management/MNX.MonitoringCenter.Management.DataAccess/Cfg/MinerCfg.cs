using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.DataAccess.Cfg;

internal class MinerCfg : IEntityTypeConfiguration<Miner>
{
    public void Configure(EntityTypeBuilder<Miner> builder)
    {
        builder.HasKey(x => x.Name);
    }
}

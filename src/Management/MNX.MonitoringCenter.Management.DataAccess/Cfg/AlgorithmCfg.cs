using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.DataAccess.Cfg;

internal class AlgorithmCfg : IEntityTypeConfiguration<Algorithm>
{
    public void Configure(EntityTypeBuilder<Algorithm> builder)
    {
        builder.HasKey(x => x.Name);
    }
}
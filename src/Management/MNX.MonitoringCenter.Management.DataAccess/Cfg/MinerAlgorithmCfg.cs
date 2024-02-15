using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.DataAccess.Dto;

namespace MNX.MonitoringCenter.Management.DataAccess.Cfg;

internal class MinerAlgorithmCfg : IEntityTypeConfiguration<MinerAlgorithm>
{
    public void Configure(EntityTypeBuilder<MinerAlgorithm> builder)
    {
        builder.HasKey(x => new { x.MinerName, x.AlgorithmName });

        builder.HasOne<MinerDto>()
               .WithMany(x => x.Algorithms)
               .HasForeignKey(x => x.MinerName);

        builder.HasOne<Algorithm>()
               .WithMany()
               .HasForeignKey(x => x.AlgorithmName);
    }
}

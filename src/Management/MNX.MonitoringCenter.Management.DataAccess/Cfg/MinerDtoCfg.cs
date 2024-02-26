using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.DataAccess.Dto;

namespace MNX.MonitoringCenter.Management.DataAccess.Cfg;

internal class MinerDtoCfg : IEntityTypeConfiguration<MinerDto>
{
    public void Configure(EntityTypeBuilder<MinerDto> builder)
    {
        builder.HasKey(x => x.Name);

        builder.HasMany<MinerAlgorithm>()
               .WithOne()
               .HasForeignKey(x => x.MinerName);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.DataAccess.Dto;

namespace MINUX.Backend.Worker.DataAccess.Cfg;

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

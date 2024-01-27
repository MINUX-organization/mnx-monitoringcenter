using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINUX.Backend.Worker.DataAccess.Dto;

namespace MINUX.Backend.Worker.DataAccess.Cfg;

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
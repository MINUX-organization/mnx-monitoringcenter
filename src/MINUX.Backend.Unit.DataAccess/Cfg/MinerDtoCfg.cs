using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINUX.Backend.Unit.DataAccess.Dto;

namespace MINUX.Backend.Unit.DataAccess.Cfg;

public class MinerDtoCfg : IEntityTypeConfiguration<MinerDto>
{
    public void Configure(EntityTypeBuilder<MinerDto> builder)
    {
        builder.HasKey(x => x.Name);

        builder.HasMany<MinerAlgorithm>()
               .WithOne()
               .HasForeignKey(x => x.MinerName);
    }
}
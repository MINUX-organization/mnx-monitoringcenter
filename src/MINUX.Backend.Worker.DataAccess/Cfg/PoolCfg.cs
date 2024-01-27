using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINUX.Backend.Worker.Core;

namespace MINUX.Backend.Worker.DataAccess.Cfg;

internal class PoolCfg : IEntityTypeConfiguration<Pool>
{
    public void Configure(EntityTypeBuilder<Pool> builder)
    {
        builder.HasOne<Cryptocurrency>()
               .WithMany()
               .HasForeignKey(x => x.Cryptocurrency);
    }
}

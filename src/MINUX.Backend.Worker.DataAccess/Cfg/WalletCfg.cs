using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINUX.Backend.Worker.Core;

namespace MINUX.Backend.Worker.DataAccess.Cfg;

public class WalletCfg : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.HasOne<Cryptocurrency>()
               .WithMany()
               .HasForeignKey(x => x.Cryptocurrency);
    }
}

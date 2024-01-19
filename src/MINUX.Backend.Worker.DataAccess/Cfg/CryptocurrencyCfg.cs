using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINUX.Backend.Worker.Core;

namespace MINUX.Backend.Worker.DataAccess.Cfg;

public class CryptocurrencyCfg : IEntityTypeConfiguration<Cryptocurrency>
{
    public void Configure(EntityTypeBuilder<Cryptocurrency> builder)
    {
        builder.HasOne<Algorithm>()
               .WithMany()
               .HasForeignKey(x => x.AlgorithmName);

        builder.Property(x => x.AlgorithmName).IsRequired();
    }
}
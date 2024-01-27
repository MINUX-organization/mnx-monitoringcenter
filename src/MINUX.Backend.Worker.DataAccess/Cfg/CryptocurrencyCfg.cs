using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINUX.Backend.Worker.Core;

namespace MINUX.Backend.Worker.DataAccess.Cfg;

internal class CryptocurrencyCfg : IEntityTypeConfiguration<Cryptocurrency>
{
    public void Configure(EntityTypeBuilder<Cryptocurrency> builder)
    {
        builder.HasKey(x => x.FullName);
        builder.HasIndex(x => new { x.FullName, x.ShortName });

        builder.HasOne<Algorithm>()
               .WithMany()
               .HasForeignKey(x => x.AlgorithmName);

        builder.Property(x => x.AlgorithmName).IsRequired();
    }
}
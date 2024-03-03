using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.DataAccess.Cfg;

internal class CryptocurrencyCfg : IEntityTypeConfiguration<Cryptocurrency>
{
    public void Configure(EntityTypeBuilder<Cryptocurrency> builder)
    {
        builder.HasIndex(x => new {x.Id, x.FullName, x.ShortName });

        builder.HasOne<Algorithm>()
               .WithMany()
               .HasForeignKey(x => x.Algorithm);

        builder.Property(x => x.Algorithm).IsRequired();
    }
}
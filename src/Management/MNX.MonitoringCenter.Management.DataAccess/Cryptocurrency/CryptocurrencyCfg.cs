using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.Cryptocurrency;

/// <summary>
/// Конфигурация для таблицы с криптовалютами.
/// </summary>
internal class CryptocurrencyCfg : IEntityTypeConfiguration<Core.Mining.Cryptocurrency>
{
    public void Configure(EntityTypeBuilder<Core.Mining.Cryptocurrency> builder)
    {
        builder.HasIndex(x => x.OwnerId);
        builder.HasIndex(x => x.ShortName);
        builder.HasIndex(x => x.FullName);
    }
}
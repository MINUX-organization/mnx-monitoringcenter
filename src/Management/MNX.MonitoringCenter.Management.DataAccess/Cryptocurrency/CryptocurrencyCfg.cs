using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.Cryptocurrency;

/// <summary>
/// Конфигурация для таблицы с криптовалютами.
/// </summary>
internal class CryptocurrencyCfg : IEntityTypeConfiguration<Core.Cryptocurrency>
{
    public void Configure(EntityTypeBuilder<Core.Cryptocurrency> builder)
    {
        builder.HasIndex(x => x.UserId);
    }
}
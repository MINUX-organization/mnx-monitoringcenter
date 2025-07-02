using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.Wallet;

/// <summary>
/// Конфигурация для таблицы с кошельками.
/// </summary>
internal class WalletCfg : IEntityTypeConfiguration<Core.Mining.Wallet>
{
    public void Configure(EntityTypeBuilder<Core.Mining.Wallet> builder)
    {
        builder.HasIndex(x => x.OwnerId);
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.Address);
    }
}
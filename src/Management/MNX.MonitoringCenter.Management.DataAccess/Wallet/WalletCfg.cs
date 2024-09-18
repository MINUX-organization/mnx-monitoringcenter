using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.Wallet;

/// <summary>
/// Конфигурация для таблицы с кошельками.
/// </summary>
internal class WalletCfg : IEntityTypeConfiguration<Core.Wallet>
{
    public void Configure(EntityTypeBuilder<Core.Wallet> builder)
    {
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.Address);
    }
}
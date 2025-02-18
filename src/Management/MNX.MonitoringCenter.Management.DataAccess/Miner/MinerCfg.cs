using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.Miner;

/// <summary>
/// Конфигурация для таблицы с майнерами.
/// </summary>
internal class MinerCfg : IEntityTypeConfiguration<Core.Mining.Miner.Miner>
{
    public void Configure(EntityTypeBuilder<Core.Mining.Miner.Miner> builder)
    {
        builder.HasIndex(x => x.Name);

        builder.Property(x => x.MiningMode)
            .HasConversion<string>();

        builder.Property(x => x.Type)
            .HasConversion<string>();
    }
}

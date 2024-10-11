using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.Miner.Cfg;

/// <summary>
/// Конфигурация для таблицы с майнерами.
/// </summary>
internal class MinerCfg : IEntityTypeConfiguration<Core.Miner.Miner>
{
    public void Configure(EntityTypeBuilder<Core.Miner.Miner> builder)
    {
        builder.HasIndex(x => x.Name);

        builder.Property(x => x.MiningMode)
            .HasConversion<string>();
    }
}

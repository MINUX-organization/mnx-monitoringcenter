using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.Miner;

/// <summary>
/// Конфигурация для таблицы с майнерами.
/// </summary>
internal class MinerCfg : IEntityTypeConfiguration<Core.Miner>
{
    public void Configure(EntityTypeBuilder<Core.Miner> builder)
    {
        builder.HasKey(x => x.Name);
    }
}

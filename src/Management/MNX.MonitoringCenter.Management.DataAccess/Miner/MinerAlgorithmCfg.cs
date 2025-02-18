using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core.Mining.Miner;

namespace MNX.MonitoringCenter.Management.DataAccess.Miner;

/// <summary>
/// Конфигурация для таблицы с алгоритмами майнеров.
/// </summary>
internal class MinerAlgorithmCfg : IEntityTypeConfiguration<MinerAlgorithm>
{
    public void Configure(EntityTypeBuilder<MinerAlgorithm> builder)
    {
        builder.HasKey(x => new { x.MinerId, x.AlgorithmId });
    }
}

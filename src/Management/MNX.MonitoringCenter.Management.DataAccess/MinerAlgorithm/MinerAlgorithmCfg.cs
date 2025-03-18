using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.MinerAlgorithm;

using MinerAlgorithm = Core.Mining.Miner.MinerAlgorithm;

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

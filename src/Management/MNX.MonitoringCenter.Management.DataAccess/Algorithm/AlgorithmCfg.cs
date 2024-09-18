using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.Algorithm;

/// <summary>
/// Конфигурация для таблицы с алгоритмами.
/// </summary>
internal class AlgorithmCfg : IEntityTypeConfiguration<Core.Algorithm>
{
    public void Configure(EntityTypeBuilder<Core.Algorithm> builder)
    {
        builder.HasIndex(x => x.Name);
    }
}
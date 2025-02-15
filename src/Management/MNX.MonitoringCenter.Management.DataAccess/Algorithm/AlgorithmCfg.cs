using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.Algorithm;

using Algorithm = Core.Mining.Algorithm;

/// <summary>
/// Конфигурация для таблицы с алгоритмами.
/// </summary>
internal class AlgorithmCfg : IEntityTypeConfiguration<Algorithm>
{
    public void Configure(EntityTypeBuilder<Algorithm> builder)
    {
        builder.HasIndex(x => new { x.Name, x.UserId }).IsUnique();
    }
}
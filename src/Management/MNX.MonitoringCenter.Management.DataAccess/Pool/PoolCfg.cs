using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.Pool;

/// <summary>
/// Конфигурация для таблицы с пулами.
/// </summary>
internal class PoolCfg : IEntityTypeConfiguration<Core.Mining.Pool>
{
    public void Configure(EntityTypeBuilder<Core.Mining.Pool> builder)
    {
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => new { x.Domain, x.Port });
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.Pool;

/// <summary>
/// Конфигурация для таблицы с пулами.
/// </summary>
internal class PoolCfg : IEntityTypeConfiguration<Core.Pool>
{
    public void Configure(EntityTypeBuilder<Core.Pool> builder)
    {
        builder.HasIndex(x => x.UserId);
    }
}

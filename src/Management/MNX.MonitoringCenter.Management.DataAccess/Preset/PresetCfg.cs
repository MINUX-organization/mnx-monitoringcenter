using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.Preset;

/// <summary>
/// Конфигурация для таблицы с пресетами.
/// </summary>
internal class PresetCfg : IEntityTypeConfiguration<Core.Preset>
{
    public void Configure(EntityTypeBuilder<Core.Preset> builder)
    {
        builder.HasIndex(x => x.UserId);
    }
}

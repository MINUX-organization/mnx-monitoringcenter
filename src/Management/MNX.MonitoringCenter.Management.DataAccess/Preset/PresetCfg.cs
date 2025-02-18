using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Management.DataAccess.Preset;

/// <summary>
/// Конфигурация для таблицы с пресетами.
/// </summary>
internal class PresetCfg : IEntityTypeConfiguration<Core.Overclocking.Preset>
{
    public void Configure(EntityTypeBuilder<Core.Overclocking.Preset> builder)
    {
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.DeviceName);

        builder.Ignore(x => x.Overclocking);
    }
}

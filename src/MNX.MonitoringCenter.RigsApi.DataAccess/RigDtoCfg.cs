using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.RigsApi.DataAccess;

/// <summary>
/// Конфигурация таблицы с ригами.
/// </summary>
internal class RigDtoCfg : IEntityTypeConfiguration<RigDto>
{
    public void Configure(EntityTypeBuilder<RigDto> builder)
    {
        builder.Property(x => x.LifeCycleStatus).HasConversion<string>();
        builder.Property(x => x.MiningLifeCycleStatus).HasConversion<string>();
    }
}

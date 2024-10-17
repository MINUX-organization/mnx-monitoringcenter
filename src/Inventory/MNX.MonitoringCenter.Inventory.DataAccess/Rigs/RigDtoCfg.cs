using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs;

/// <summary>
/// Конфигурация таблицы с ригами.
/// </summary>
internal class RigDtoCfg : IEntityTypeConfiguration<RigDto>
{
    public void Configure(EntityTypeBuilder<RigDto> builder)
    {
        builder.Ignore(x => x.CurrentInventory);
    }
}

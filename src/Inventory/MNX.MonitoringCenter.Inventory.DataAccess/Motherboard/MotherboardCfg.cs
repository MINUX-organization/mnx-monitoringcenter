using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Motherboard;

/// <summary>
/// Конфигурация для таблицы с материнскими платами.
/// </summary>
internal class MotherboardCfg : IEntityTypeConfiguration<Contracts.Motherboard.Motherboard>
{
    public void Configure(EntityTypeBuilder<Contracts.Motherboard.Motherboard> builder)
    {
        builder.ComplexProperty(e => e.Information);
    }
}

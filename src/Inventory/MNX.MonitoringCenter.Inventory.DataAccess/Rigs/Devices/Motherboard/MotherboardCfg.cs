using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Inventory.DataAccess.RigInventory.Devices.Motherboard;

/// <summary>
/// Конфигурация для таблицы с материнскими платами.
/// </summary>
internal class MotherboardCfg : IEntityTypeConfiguration<Contracts.Motherboard.Motherboard>
{
    public void Configure(EntityTypeBuilder<Contracts.Motherboard.Motherboard> builder)
    {
        builder.ComplexProperty(e => e.Information);

        builder.HasMany(x => x.Pcies)
               .WithOne()
               .HasForeignKey("motherboard_id");
    }
}

/// <summary>
/// Конфигурация для таблицы pci материнской платы.
/// </summary>
internal class MotherboardPciCfg : IEntityTypeConfiguration<Contracts.Motherboard.MotherboardPci>
{
    public void Configure(EntityTypeBuilder<Contracts.Motherboard.MotherboardPci> builder)
    {
        builder.HasKey("Id", "motherboard_id");
    }
}

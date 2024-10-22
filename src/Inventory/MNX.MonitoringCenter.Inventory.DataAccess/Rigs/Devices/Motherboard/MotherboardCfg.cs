using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Motherboard;

namespace MNX.MonitoringCenter.Inventory.DataAccess.RigInventory.Devices.Motherboard;

/// <summary>
/// Конфигурация для таблицы с материнскими платами.
/// </summary>
internal class MotherboardCfg : IEntityTypeConfiguration<Contracts.Devices.Motherboard.Motherboard>
{
    public void Configure(EntityTypeBuilder<Contracts.Devices.Motherboard.Motherboard> builder)
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
internal class MotherboardPciCfg : IEntityTypeConfiguration<MotherboardPci>
{
    public void Configure(EntityTypeBuilder<MotherboardPci> builder)
    {
        builder.HasKey("Id", "motherboard_id");
    }
}

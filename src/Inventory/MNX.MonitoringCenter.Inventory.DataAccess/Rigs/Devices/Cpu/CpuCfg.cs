using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Inventory.DataAccess.RigInventory.Devices.Cpu;

/// <summary>
/// Конфигурация таблицы с процессорами.
/// </summary>
internal class CpuCfg : IEntityTypeConfiguration<Contracts.Devices.Cpu.Cpu>
{
    public void Configure(EntityTypeBuilder<Contracts.Devices.Cpu.Cpu> builder)
    {
        builder.HasKey("RigInventoryId", "Id");

        builder.ComplexProperty(e => e.Pci);

        builder.ComplexProperty(e => e.Information, x =>
        {
            x.ComplexProperty(e => e.Cache);
        });

        builder.ComplexProperty(e => e.Restrictions, x =>
        {
            x.ComplexProperty(e => e.Temperature);
            x.ComplexProperty(e => e.Power);
            x.ComplexProperty(e => e.Clock);
            x.ComplexProperty(e => e.FanSpeed);
        });

        builder.ComplexProperty(e => e.Overclocking);
    }
}

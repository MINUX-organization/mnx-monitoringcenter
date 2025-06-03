using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu;

/// <summary>
/// Конфигурация таблицы с видеокартами.
/// </summary>
internal class GpuCfg : IEntityTypeConfiguration<GpuInventoryDto>
{
    public void Configure(EntityTypeBuilder<GpuInventoryDto> builder)
    {
        builder.HasKey(x => new { x.RigInventoryId, x.Id });

        builder.HasOne<RigInventory>()
            .WithMany(x => x.Gpus)
            .HasForeignKey(x => x.RigInventoryId)
            .HasPrincipalKey(x => x.Id);

        builder.ComplexProperty(e => e.Pci);

        builder.ComplexProperty(e => e.Information, x =>
        {
            x.ComplexProperty(e => e.Technology);
            x.ComplexProperty(e => e.Memory);
        });

        builder.ComplexProperty(e => e.Restrictions, x =>
        {
            x.ComplexProperty(e => e.Power);
            x.ComplexProperty(e => e.FanSpeed);

            x.ComplexProperty(e => e.Temperature, x =>
            {
                x.ComplexProperty(e => e.Core);
                x.ComplexProperty(e => e.Memory);
            });

            x.ComplexProperty(e => e.Voltage, x =>
            {
                x.ComplexProperty(e => e.Core, x =>
                {
                    x.ComplexProperty(e => e.Lock);
                    x.ComplexProperty(e => e.Offset);
                });
                x.ComplexProperty(e => e.Memory, x =>
                {
                    x.ComplexProperty(e => e.Lock);
                    x.ComplexProperty(e => e.Offset);
                });
            });

            x.ComplexProperty(e => e.Clock, x =>
            {
                x.ComplexProperty(e => e.Core, x =>
                {
                    x.ComplexProperty(e => e.Lock);
                    x.ComplexProperty(e => e.Offset);
                });
                x.ComplexProperty(e => e.Memory, x =>
                {
                    x.ComplexProperty(e => e.Lock);
                    x.ComplexProperty(e => e.Offset);
                });
            });
        });

        builder.ComplexProperty(e => e.Overclocking);
    }
}

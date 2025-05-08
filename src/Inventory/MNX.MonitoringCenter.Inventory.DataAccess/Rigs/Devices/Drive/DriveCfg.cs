using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Drive;

/// <summary>
/// Конфигурация таблицы с дисками.
/// </summary>
internal class DriveCfg : IEntityTypeConfiguration<Contracts.Devices.Drive.Drive>
{
    public void Configure(EntityTypeBuilder<Contracts.Devices.Drive.Drive> builder)
    {
        builder.HasKey("RigInventoryId", "Id");

        builder.ComplexProperty(e => e.Information);
    }
}

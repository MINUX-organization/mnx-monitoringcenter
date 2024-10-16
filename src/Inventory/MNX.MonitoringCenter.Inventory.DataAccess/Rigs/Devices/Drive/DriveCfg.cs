using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Inventory.DataAccess.RigInventory.Devices.Drive;

/// <summary>
/// Конфигурация таблицы с дисками.
/// </summary>
internal class DriveCfg : IEntityTypeConfiguration<Contracts.Drive.Drive>
{
    public void Configure(EntityTypeBuilder<Contracts.Drive.Drive> builder)
    {
        builder.ComplexProperty(e => e.Information);
    }
}

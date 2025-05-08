using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.NetworkAdapter;

/// <summary>
/// Конфигурация для таблицы с сетевыми адаптерами.
/// </summary>
internal class NetworkAdapterCfg : IEntityTypeConfiguration<Contracts.Devices.NetworkAdapter.NetworkAdapter>
{
    public void Configure(EntityTypeBuilder<Contracts.Devices.NetworkAdapter.NetworkAdapter> builder)
    {
        builder.HasKey("RigInventoryId", "Id");

        builder.ComplexProperty(e => e.Information);
    }
}

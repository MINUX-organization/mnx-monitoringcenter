using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Inventory.DataAccess.NetworkAdapter;

/// <summary>
/// Конфигурация для таблицы с сетевыми адаптерами.
/// </summary>
internal class NetworkAdapterCfg : IEntityTypeConfiguration<Contracts.NetworkAdapter.NetworkAdapter>
{
    public void Configure(EntityTypeBuilder<Contracts.NetworkAdapter.NetworkAdapter> builder)
    {
        builder.ComplexProperty(e => e.Information);
    }
}

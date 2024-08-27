using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MNX.MonitoringCenter.Inventory.DataAccess.InternetAdapter;

/// <summary>
/// Конфигурация для таблицы с интернет адаптерами.
/// </summary>
internal class InternetAdapterCfg : IEntityTypeConfiguration<Contracts.InternetAdapter.InternetAdapter>
{
    public void Configure(EntityTypeBuilder<Contracts.InternetAdapter.InternetAdapter> builder)
    {
        builder.ComplexProperty(e => e.Information);
    }
}

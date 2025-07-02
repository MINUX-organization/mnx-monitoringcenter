using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Views;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Cfg;

/// <summary>
/// Конфигурация таблицы представления <see cref="GpuInventoryView"/>.
/// </summary>
internal class GpuInventoryViewCfg : IEntityTypeConfiguration<GpuInventoryView>
{
    ///
    public void Configure(EntityTypeBuilder<GpuInventoryView> builder)
    {
        builder.ToView("gpu_info_view", "monitoring_center");

        builder.OwnsOne(x => x.Pci);
        builder.OwnsOne(x => x.Information, information =>
        {
            information.OwnsOne(x => x.Technology);
            information.OwnsOne(x => x.Memory);
        });

        builder.OwnsOne(x => x.NvidiaRestrictions, rest =>
        {
            rest.ConfigureBaseRestrictionsParams();
            rest.ConfigureNvidiaRestrictionsParams();
        });

        builder.OwnsOne(x => x.AmdRestrictions, rest =>
        {
            rest.ConfigureBaseRestrictionsParams();
            rest.ConfigureAmdRestrictionsParams();
        });

        builder.OwnsOne(x => x.IntelRestrictions, rest =>
        {
            rest.ConfigureBaseRestrictionsParams();
        });

        builder.OwnsOne(x => x.NvidiaOverclocking);
        builder.OwnsOne(x => x.AmdOverclocking);
    }
}

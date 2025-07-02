using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Views;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Cfg;

/// <summary>
/// Конфигурация таблицы представления <see cref="GpuRestrictionsView"/>.
/// </summary>
public class GpuRestrictionsViewCfg : IEntityTypeConfiguration<GpuRestrictionsView>
{
    ///
    public void Configure(EntityTypeBuilder<GpuRestrictionsView> builder)
    {
        builder.ToView("gpu_restrictions_view", "monitoring_center");
        
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.RigInventoryId).HasColumnName("rig_inventory_id");
        builder.Property(x => x.Manufacturer).HasColumnName("manufacturer");
        builder.Property(x => x.Model).HasColumnName("model");

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
    }
}
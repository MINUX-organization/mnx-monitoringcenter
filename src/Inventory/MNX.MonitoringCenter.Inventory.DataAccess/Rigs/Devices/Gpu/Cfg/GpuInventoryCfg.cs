using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Cfg;

/// <summary>
/// Конфигурация таблицы сущности <see cref="GpuInventory"/>.
/// </summary>
internal class GpuInventoryCfg : IEntityTypeConfiguration<GpuInventory>
{
    ///
    public void Configure(EntityTypeBuilder<GpuInventory> builder)
    {
        builder.ToTable("gpu", "monitoring_center");
        builder.HasKey(x => new { x.RigInventoryId, x.Id });

        builder.HasOne<RigInventory>()
            .WithMany(x => x.Gpus)
            .HasForeignKey(x => x.RigInventoryId)
            .HasPrincipalKey(x => x.Id);

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

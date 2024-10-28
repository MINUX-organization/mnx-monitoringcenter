using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu;
using System.Reflection;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Контекст базы данных.
/// </summary>
public class Context : DbContext
{
    /// <summary>
    /// Риги.
    /// </summary>
    internal DbSet<RigDto> Rigs { get; set; }

    /// <summary>
    /// Записи инвентаризаций.
    /// </summary>
    internal DbSet<RigInventory.RigInventory> RigInventory { get; set; }

    /// <summary>
    /// Видеокарты.
    /// </summary>
    internal DbSet<Gpu> Gpu { get; set; }

    public Context(DbContextOptions<Context> options) : base(options) { }

    /// <summary>
    /// Получить версию драйвера для видеокарт переданного производителя.
    /// </summary>
    /// <param name="amdDriverVersion"> Версия AMD драйвера. </param>
    /// <param name="intelDriverVersion"> Версия Intel драйвера. </param>
    /// <param name="nvidiaDriverVersion"> Версия Nvidia драйвера. </param>
    /// <param name="gpuManufacturer"> Производитель видеокарты. </param>
    /// <returns>
    /// Если производитель видеокарты поддерживается системой, то вернётся версия драйвера для видеокарт этого производителя,
    /// иначе вернётся <see langword="null"/>.
    /// </returns>
    public static string? GetGpuDriverVersion(string amdDriverVersion,
                                              string intelDriverVersion,
                                              string nvidiaDriverVersion,
                                              string gpuManufacturer)
    {
        if (Enum.TryParse<SupportedGpuManufacturer>(gpuManufacturer, out var manufacturer))
        {
            return manufacturer switch
            {
                SupportedGpuManufacturer.AMD => amdDriverVersion,
                SupportedGpuManufacturer.Intel => intelDriverVersion,
                SupportedGpuManufacturer.Nvidia => nvidiaDriverVersion,
                _ => null
            };
        }

        return null;
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.HasDbFunction(typeof(Context).GetMethod(nameof(GetGpuDriverVersion))!)
                    .HasName("get_gpu_driver_version");
    }
}

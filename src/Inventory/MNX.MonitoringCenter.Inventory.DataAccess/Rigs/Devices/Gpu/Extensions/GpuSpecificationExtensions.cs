using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Extensions;

/// <summary>
/// Расширения для <see cref="IQueryable{Gpu}"/>, для обработки спецификации.
/// </summary>
internal static class GpuSpecificationExtensions
{
    /// <summary>
    /// Фильтровать.
    /// </summary>
    /// <param name="gpus"> Запрашиваемый список видеокарт. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Запрашиваемый список видеокарт. </returns>
    internal static IQueryable<GpuInventory> Filter(this IQueryable<GpuInventory> gpus, DeviceSpecification specification)
    {
        return gpus.Filter<GpuInventory>(specification);
    }

    private static IQueryable<TGpu> Filter<TGpu>(this IQueryable<TGpu> gpus, DeviceSpecification specification)
        where TGpu : GpuInventory
    {
        if (specification.Models != null)
        {
            gpus = gpus.Where(x => specification.Models.Contains(x.Information.Model));
        }

        if (specification.Manufacturers != null)
        {
            gpus = gpus.Where(x => specification.Manufacturers.Contains(x.Information.Manufacturer));
        }

        return gpus;
    }
}

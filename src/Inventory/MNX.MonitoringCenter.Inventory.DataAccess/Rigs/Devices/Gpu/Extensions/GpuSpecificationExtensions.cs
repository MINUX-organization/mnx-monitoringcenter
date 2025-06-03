using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusDetails;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Extensions;

using Gpu = Contracts.Devices.Gpu.Gpu;

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
    internal static IQueryable<Gpu> Filter(this IQueryable<Gpu> gpus, DeviceSpecification specification)
    {
        return gpus.Filter<Gpu>(specification);

    }

    /// <summary>
    /// Фильтровать.
    /// </summary>
    /// <param name="gpus"> Запрашиваемый список видеокарт. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Запрашиваемый список видеокарт. </returns>
    internal static IQueryable<GpuDetails> Filter(this IQueryable<GpuDetails> gpus, DeviceSpecification specification)
    {
        return gpus.Filter<GpuDetails>(specification);
    }

    private static IQueryable<TGpu> Filter<TGpu>(this IQueryable<TGpu> gpus, DeviceSpecification specification)
        where TGpu : Gpu
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

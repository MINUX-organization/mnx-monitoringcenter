using MNX.MonitoringCenter.Inventory.Contracts.Queries;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Gpu;

using Gpu = Contracts.Gpu.Gpu;

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

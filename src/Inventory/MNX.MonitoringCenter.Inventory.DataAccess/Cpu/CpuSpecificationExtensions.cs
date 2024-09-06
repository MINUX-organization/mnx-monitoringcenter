using MNX.MonitoringCenter.Inventory.UseCases;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Cpu;

using Cpu = Contracts.Cpu.Cpu;

/// <summary>
/// Расширения для <see cref="IQueryable{Cpu}"/>, для обработки спецификации.
/// </summary>
internal static class CpuSpecificationExtensions
{
    /// <summary>
    /// Фильтровать.
    /// </summary>
    /// <param name="cpus"> Запрашиваемый список процессоров. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Запрашиваемый список процессоров. </returns>
    internal static IQueryable<Cpu> Filter(this IQueryable<Cpu> cpus, DeviceSpecification specification)
    {
        if (specification.Models != null)
        {
            cpus = cpus.Where(x => specification.Models.Contains(x.Information.Model));
        }

        if (specification.Manufacturers != null)
        {
            cpus = cpus.Where(x => specification.Manufacturers.Contains(x.Information.Manufacturer));
        }

        return cpus;
    }
}

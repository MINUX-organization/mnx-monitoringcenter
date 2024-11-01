using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu.GetCpusDetails;

namespace MNX.MonitoringCenter.Inventory.DataAccess.RigInventory.Devices.Cpu;

using Cpu = Contracts.Devices.Cpu.Cpu;

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
        return cpus.Filter<Cpu>(specification);
    }

    /// <summary>
    /// Фильтровать.
    /// </summary>
    /// <param name="cpus"> Запрашиваемый список процессоров. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Запрашиваемый список процессоров. </returns>
    internal static IQueryable<CpuDetails> Filter(this IQueryable<CpuDetails> cpus, DeviceSpecification specification)
    {
        return cpus.Filter<CpuDetails>(specification);
    }

    private static IQueryable<TCpu> Filter<TCpu>(this IQueryable<TCpu> cpus, DeviceSpecification specification)
        where TCpu : Cpu
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

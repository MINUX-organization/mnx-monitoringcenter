using MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Dto.Devices.Gpu;

/// <summary>
/// Видеокарта.
/// </summary>
public class GpuDto : MiningDeviceDto
{
    /// <summary>
    /// Идентификатор PCI шины.
    /// </summary>
    public int PciBusId { get; set; }

    /// <summary>
    /// Критическая температура.
    /// </summary>
    public int CriticalTemperature { get; set; }

    /// <summary>
    /// Ограничение мощности.
    /// </summary>
    public int PowerLimit { get; set; }

    /// <summary>
    /// Технология параллельных вычислений.
    /// </summary>  
    public ParallelComputingTechnologyEnum Technology { get; set; }

    /// <summary>
    /// Версия технологии параллельных вычислений.
    /// </summary>
    public string TechnologyVersion { get; set; }

    /// <summary>
    /// Продавец.
    /// </summary>
    public string Vendor { get; set; }

    /// <summary>
    /// Размер памяти.
    /// </summary>
    public int MemorySize { get; set; }

    /// <summary>
    /// Продавец памяти.
    /// </summary>
    public string MemoryVendor { get; set; }

    /// <summary>
    /// Тип памяти.
    /// </summary>
    public string MemoryType { get; set; }

    /// <summary>
    /// Версия биоса.
    /// </summary>
    public string BiosVersion { get; set; }

    /// <summary>
    /// Идентификатор разгона.
    /// </summary>
    public Guid? OverclockingId { get; set; }
}

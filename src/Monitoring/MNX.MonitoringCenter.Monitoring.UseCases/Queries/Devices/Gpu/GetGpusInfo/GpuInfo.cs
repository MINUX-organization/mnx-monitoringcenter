using MNX.MonitoringCenter.Monitoring.Core.Devices;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.Gpu.GetGpusInfo;

/// <summary>
/// Информация о видеокарте.
/// </summary>
public class GpuInfo : MiningDeviceInfo
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
    public ParallelComputingTechnology Technology { get; set; }

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
    /// Версия драйвера.
    /// </summary>
    public string DriverVersion { get; set; }
}

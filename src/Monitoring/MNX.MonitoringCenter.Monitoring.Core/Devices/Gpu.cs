using MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;

namespace MNX.MonitoringCenter.Monitoring.Core.Devices;

/// <summary>
/// Видеокарта.
/// </summary>
public class Gpu : MiningDevice
{
    /// <inheritdoc/>
    public override MiningDeviceType Type
    {
        get => MiningDeviceType.GPU;
    }

    /// <summary>
    /// Идентификатор PCI шины.
    /// </summary>
    public int PciBusId { get; }

    /// <summary>
    /// Критическая температура.
    /// </summary>
    public int CriticalTemperature { get; }

    /// <summary>
    /// Ограничение мощности.
    /// </summary>
    public int PowerLimit { get; }

    /// <summary>
    /// Технология параллельных вычислений.
    /// </summary>
    public ParallelComputingTechnology Technology { get; }

    /// <summary>
    /// Продавец.
    /// </summary>
    public string Vendor { get; }

    /// <summary>
    /// Размер памяти.
    /// </summary>
    public int MemorySize { get; }

    /// <summary>
    /// Продавец памяти.
    /// </summary>
    public string MemoryVendor { get; }

    /// <summary>
    /// Тип памяти.
    /// </summary>
    public string MemoryType { get; }

    /// <summary>
    /// Версия биоса.
    /// </summary>
    public string BiosVersion { get; }

    /// <summary>
    /// Версия драйвера.
    /// </summary>
    public string DriverVersion { get; }

    public Gpu(Guid id, string name, string rigName, GpuManufacturerEnum manufacturer,
               string serialNumber, int pciBusId, int criticalTemperature, int powerLimit,
               ParallelComputingTechnology technology, string vendor, int memorySize,
               string memoryVendor, string memoryType, string biosVersion, string driverVersion)
        : base(id, name, rigName, manufacturer.ToString(), serialNumber)
    {
        PciBusId = pciBusId;
        CriticalTemperature = criticalTemperature;
        PowerLimit = powerLimit;
        Technology = technology;
        Vendor = vendor;
        MemorySize = memorySize;
        MemoryVendor = memoryVendor;
        MemoryType = memoryType;
        BiosVersion = biosVersion;
        DriverVersion = driverVersion;
    }
}

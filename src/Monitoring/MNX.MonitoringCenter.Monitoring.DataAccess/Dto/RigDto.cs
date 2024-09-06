using MNX.MonitoringCenter.Monitoring.DataAccess.Dto.Devices;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Dto;

/// <summary>
/// Риг.
/// </summary>
public class RigDto
{
    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Глобальный IP-адрес.
    /// </summary>
    public string GlobalIP { get; set; }

    /// <summary>
    /// Локальный IP-адрес.
    /// </summary>
    public string LocalIP { get; set; }

    /// <summary>
    /// MAC адрес.
    /// </summary>
    public string Mac { get; set; }

    /// <summary>
    /// Версия Minux.
    /// </summary>
    public string MinuxVersion { get; set; }

    /// <summary>
    /// Версия Linux.
    /// </summary>
    public string LinuxVersion { get; set; }

    /// <summary>
    /// Версия AMD драйвера.
    /// </summary>
    public string AmdDriverVersion { get; set; }

    /// <summary>
    /// Версия драйвера Nvidia.
    /// </summary>
    public string NvidiaDriverVersion { get; set; }

    /// <summary>
    /// Версия драйвера Intel.
    /// </summary>
    public string IntelDriverVersion { get; set; }

    /// <summary>
    /// Версия OpenCL.
    /// </summary>
    public string OpenCLVersion { get; set; }

    /// <summary>
    /// Версия CUDA.
    /// </summary>
    public string CudaVersion { get; set; }

    /// <summary>
    /// Кол-во карт Amd.
    /// </summary>
    public int AmdGpusCount
    {
        get => Devices.Count(device => device.Type == Core.Devices.Enums.MiningDeviceType.GPU
                                    && device.Manufacturer == Core.Devices.Enums.GpuManufacturerEnum.Amd.ToString());
    }

    /// <summary>
    /// Кол-во карт Nvidia.
    /// </summary>
    public int NvidiaGpusCount
    {
        get => Devices.Count(device => device.Type == Core.Devices.Enums.MiningDeviceType.GPU
                                    && device.Manufacturer == Core.Devices.Enums.GpuManufacturerEnum.Nvidia.ToString());
    }

    /// <summary>
    /// Кол-во карт Intel.
    /// </summary>
    public int IntelGpusCount
    {
        get => Devices.Count(device => device.Type == Core.Devices.Enums.MiningDeviceType.GPU
                                    && device.Manufacturer == Core.Devices.Enums.GpuManufacturerEnum.Intel.ToString());
    }

    /// <summary>
    /// Кол-во процессоров Amd.
    /// </summary>
    public int AmdCpusCount
    {
        get => Devices.Count(device => device.Type == Core.Devices.Enums.MiningDeviceType.CPU
                                    && device.Manufacturer == Core.Devices.Enums.CpuManufacturerEnum.Amd.ToString());
    }

    /// <summary>
    /// Кол-во процессоров Intel.
    /// </summary>
    public int IntelCpusCount
    {
        get => Devices.Count(device => device.Type == Core.Devices.Enums.MiningDeviceType.CPU
                                    && device.Manufacturer == Core.Devices.Enums.CpuManufacturerEnum.Intel.ToString());
    }

    /// <summary>
    /// Кол-во жёстких дисков.
    /// </summary>
    public int HddsCount
    {
        get => Devices.Count(device => device.Type == Core.Devices.Enums.MiningDeviceType.HDD);
    }

    /// <summary>
    /// Майнинг устройства.
    /// </summary>
    public List<MiningDeviceDto> Devices { get; set; } = new();
}

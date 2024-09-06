using MNX.MonitoringCenter.Monitoring.Core.Devices.Abstractions;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;

namespace MNX.MonitoringCenter.Monitoring.Core;

/// <summary>
/// Риг.
/// </summary>
public class Rig
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
    /// Общее кол-во видеокарт.
    /// </summary>
    private TotalGpusCount? _totalGpusCount;
    public TotalGpusCount TotalGpusCount
    {
        get
        {
            if (_totalGpusCount is null)
            {
                var gpus = Devices.Where(device => device.Type == MiningDeviceType.GPU);

                _totalGpusCount = new TotalGpusCount()
                {
                    Amd = gpus.Count(gpu => gpu.Manufacturer == GpuManufacturerEnum.Amd.ToString()),
                    Nvidia = gpus.Count(gpu => gpu.Manufacturer == GpuManufacturerEnum.Nvidia.ToString()),
                    Intel = gpus.Count(gpu => gpu.Manufacturer == GpuManufacturerEnum.Intel.ToString())
                };
            }
            
            return _totalGpusCount;
        }
    }

    /// <summary>
    /// Общее кол-во процессоров.
    /// </summary>
    private TotalCpusCount? _totalCpusCount;
    public TotalCpusCount TotalCpusCount
    {
        get
        {
            if (_totalCpusCount is null)
            {
                var cpus = Devices.Where(device => device.Type == MiningDeviceType.CPU);

                _totalCpusCount = new TotalCpusCount()
                {
                    Amd = cpus.Count(cpu => cpu.Manufacturer == CpuManufacturerEnum.Amd.ToString()),
                    Intel = cpus.Count(cpu => cpu.Manufacturer == CpuManufacturerEnum.Intel.ToString())
                };
            }

            return _totalCpusCount;
        }
    }

    /// <summary>
    /// Общее кол-во жёстких дисков.
    /// </summary>
    private int? _totalHddsCount;
    public int TotalHddsCount
    {
        get
        {
            _totalHddsCount ??= Devices.Count(device => device.Type == MiningDeviceType.HDD);
            return _totalHddsCount.Value;
        }
    }

    /// <summary>
    /// Майнинг устройства.
    /// </summary>
    public List<MiningDevice> Devices { get; set; } = new();
}

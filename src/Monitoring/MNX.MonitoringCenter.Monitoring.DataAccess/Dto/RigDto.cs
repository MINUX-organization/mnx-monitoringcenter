using MNX.MonitoringCenter.Monitoring.Core;

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
    public long UserId { get; set; }

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
    public string LisnuxVersion { get; set; }

    /// <summary>
    /// Версия AMD драйвера.
    /// </summary>
    public string AmdDriverVersion { get; set; }

    /// <summary>
    /// Версия драйвера Nvidia.
    /// </summary>
    public string NvidiaDriverVersion { get; set; }

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
    public int AmdGpusCount { get; set; }

    /// <summary>
    /// Кол-во карт Nvidia.
    /// </summary>
    public int NvidiaGpusCount { get; set; }

    /// <summary>
    /// Кол-во карт Intel.
    /// </summary>
    public int IntelGpusCount { get; set; }

    /// <summary>
    /// Кол-во процессоров Amd.
    /// </summary>
    public int AmdCpusCount { get; set; }

    /// <summary>
    /// Кол-во процессоров Intel.
    /// </summary>
    public int IntelCpusCount { get; set; }

    /// <summary>
    /// Кол-во жёстких дисков.
    /// </summary>
    public int HddsCount { get; set; }

    /// <summary>
    /// Информация о полётных листах.
    /// </summary>
    public List<FlightSheet> FlightSheetInfo { get; set; } = new();
}

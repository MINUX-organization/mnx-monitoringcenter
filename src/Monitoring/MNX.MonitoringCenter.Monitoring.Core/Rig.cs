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
    public TotalGpusCount TotalGpusCount { get; set; }

    /// <summary>
    /// Общее кол-во процессоров.
    /// </summary>
    public TotalCpusCount TotalCpusCount { get; set; }

    /// <summary>
    /// Кол-во жёстких дисков.
    /// </summary>
    public int HddsCount { get; set; }

    /// <summary>
    /// Информация о полётных листах.
    /// </summary>
    public List<FlightSheet> FlightSheetInfo { get; set; } = new();
}

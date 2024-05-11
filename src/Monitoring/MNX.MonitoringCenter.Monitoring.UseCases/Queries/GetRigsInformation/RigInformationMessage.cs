namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;

/// <summary>
/// Модель рига.
/// </summary>
public class RigInformationMessage
{
    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Индекс рига.
    /// </summary>
    public int Index { get; set; }

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
    /// Количество карт Nvidia.
    /// </summary>
    public int NvidiaGpusCount { get; set; }

    /// <summary>
    /// Количество карт Amd.
    /// </summary>
    public int AmdGpusCount { get; set; }

    /// <summary>
    /// Количество карт Intel.
    /// </summary>
    public int IntelGpusCount { get; set; }

    /// <summary>
    /// Общее количество видеокарт.
    /// </summary>
    public int TotalGpusCount { get; set; }

    /// <summary>
    /// Количество процессоров AMD.
    /// </summary>
    public int AmdCpusCount { get; set; }

    /// <summary>
    /// Количество процессоров Intel.
    /// </summary>
    public int IntelCpusCount { get; set; }

    /// <summary>
    /// Общее количество процессоров.
    /// </summary>
    public int TotalCpusCount { get; set; }

    /// <summary>
    /// Общее количество жёстких дисков.
    /// </summary>
    public int TotalHddsCount { get; set; }

    /// <summary>
    /// Майнинг устройства.
    /// </summary>
    public List<MiningDeviceModel> Devices { get; set; } = new();
}

using MNX.MonitoringCenter.Monitoring.Core;

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
    /// Локальный IP-адрес.
    /// </summary>
    public string LocalIP { get; set; }

    /// <summary>
    /// Версия Minux.
    /// </summary>
    public string MinuxVersion { get; set; }

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
    /// Информация о полётных листах.
    /// </summary>
    public List<FlightSheet> FlightSheetInfo { get; set; } = new();
}

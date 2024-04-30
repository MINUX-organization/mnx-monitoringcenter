namespace MNX.MonitoringCenter.Monitoring.Core;

/// <summary>
/// Общее кол-во видеокарт по признакам.
/// </summary>
public class TotalGpusCount
{
    /// <summary>
    /// Общее кол-во.
    /// </summary>
    public int Total
    {
        get => Amd + Nvidia + Intel;
    }

    /// <summary>
    /// Кол-во карт Amd.
    /// </summary>
    public int Amd { get; set; }

    /// <summary>
    /// Кол-во карт Nvidia.
    /// </summary>
    public int Nvidia { get; set; }

    /// <summary>
    /// Кол-во карт Intel.
    /// </summary>
    public int Intel { get; set; }

    /// <summary>
    /// Оператор сложения.
    /// </summary>
    public static TotalGpusCount operator +(TotalGpusCount first, TotalGpusCount second)
    {
        return new TotalGpusCount()
        {
            Amd = first.Amd + second.Amd,
            Nvidia = first.Nvidia + second.Nvidia,
            Intel = first.Intel + second.Intel
        };
    }
}

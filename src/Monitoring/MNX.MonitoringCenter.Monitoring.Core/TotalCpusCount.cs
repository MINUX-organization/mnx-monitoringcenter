namespace MNX.MonitoringCenter.Monitoring.Core;

/// <summary>
/// Общее кол-во процессоров по признакам.
/// </summary>
public class TotalCpusCount
{
    /// <summary>
    /// Общее кол-во.
    /// </summary>
    public int Total
    {
        get => Amd + Intel;
    }

    /// <summary>
    /// Кол-во процессоров Amd.
    /// </summary>
    public int Amd { get; set; }

    /// <summary>
    /// Кол-во процессоров Intel.
    /// </summary>
    public int Intel { get; set; }

    /// <summary>
    /// Оператор сложения.
    /// </summary>
    public static TotalCpusCount operator + (TotalCpusCount first, TotalCpusCount second)
    {
        return new TotalCpusCount()
        {
            Amd = first.Amd + second.Amd,
            Intel = first.Intel + second.Intel
        };
    }
}

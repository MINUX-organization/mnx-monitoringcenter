namespace MNX.MonitoringCenter.Management.Core;

/// <summary>
/// Криптовалюта
/// </summary>
public class Cryptocurrency
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Короткое название
    /// </summary>
    public string ShortName { get; set; }

    /// <summary>
    /// Полное название
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Алгоритма
    /// </summary>
    public string AlgorithmName { get; set; }
}
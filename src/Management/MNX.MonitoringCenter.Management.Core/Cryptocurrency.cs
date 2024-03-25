namespace MNX.MonitoringCenter.Management.Core;

/// <summary>
/// Криптовалюта
/// </summary>
public class Cryptocurrency
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Короткое название
    /// </summary>
    public string ShortName { get; set; }

    /// <summary>
    /// Полное название
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Алгоритм
    /// </summary>
    public string Algorithm { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }
}
using MNX.MonitoringCenter.Management.Core.Mining;

namespace MNX.MonitoringCenter.Management.Contracts;

/// <summary>
/// Модель для криптовалюты.
/// </summary>
public class CryptocurrencyModel
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    /// <remarks>
    /// Если null, то сущность является доменной,
    /// иначе - пользовательской.
    /// </remarks>
    public Guid? OwnerId { get; set; }

    /// <summary>
    /// Короткое название.
    /// </summary>
    public required string ShortName { get; set; }

    /// <summary>
    /// Полное название.
    /// </summary>
    public required string FullName { get; set; }
    
    /// <summary>
    /// Алгоритм.
    /// </summary>
    public required Algorithm Algorithm { get; set; }
}

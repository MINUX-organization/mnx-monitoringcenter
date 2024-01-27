namespace MINUX.Backend.Worker.Core;

/// <summary>
/// Кошулёк
/// </summary>
public class Wallet
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Адрес
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Полное название криптовалюты
    /// </summary>
    public string Cryptocurrency { get; set; }
}
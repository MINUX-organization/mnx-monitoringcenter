namespace MINUX.Backend.Worker.Core;

/// <summary>
/// Пул
/// </summary>
public class Pool
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Домен
    /// </summary>
    public string Domain {  get; set; }

    /// <summary>
    /// Порт
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Полное название криптовалюты
    /// </summary>
    public string Cryptocurrency { get; set; }
}
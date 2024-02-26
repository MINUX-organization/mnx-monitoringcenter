using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Abstractions;

/// <summary>
/// Интерфейс репозитория для доступа к криптовалютам
/// </summary>
public interface ICryptocurrencyRepository
{
    /// <summary>
    /// Получить список всех добавленных криптовалют
    /// </summary>
    /// <returns></returns>
    IAsyncEnumerable<Cryptocurrency> GetAll();

    /// <summary>
    /// Получить криптовалюту по полному названию
    /// </summary>
    /// <param name="fullName"> Полное название монеты </param>
    /// <returns> Криптовалюта </returns>
    Task<Cryptocurrency?> GetByFullName(string fullName);

    /// <summary>
    /// Проверить наличие криптовалюты по полному и/или короткому названию
    /// </summary>
    /// <param name="id">  Уникальный идентификатор </param>
    /// <returns>
    /// <see langword="true"/>, если криптовалюта была найдена хотя бы по одному параметру, иначе <see langword="false"/>
    /// </returns>
    Task<bool> Exists(string fullName, string? shortName = null);

    /// <summary>
    /// Добавить криптовалюту
    /// </summary>
    /// <param name="cryptocurrency"> Криптовалюта </param>
    Task Add(Cryptocurrency cryptocurrency);

    /// <summary>
    /// Удалить криптовалюту
    /// </summary>
    /// <param name="cryptocurrency"> Криптовалюта </param>
    Task Remove(Cryptocurrency cryptocurrency);
}
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
    /// <returns>Список криптовалют</returns>
    IAsyncEnumerable<Cryptocurrency> GetAll();

    /// <summary>
    /// Получить криптовалюту по идентификатору
    /// </summary>
    /// <param name="id"> Идентификатор монеты </param>
    /// <returns> Криптовалюта </returns>
    Task<Cryptocurrency?> GetById(int id);

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
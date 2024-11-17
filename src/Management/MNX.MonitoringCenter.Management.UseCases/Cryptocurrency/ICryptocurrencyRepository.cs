namespace MNX.MonitoringCenter.Management.UseCases.Cryptocurrency;

using Cryptocurrency = Core.Cryptocurrency;

/// <summary>
/// Интерфейс репозитория для доступа к криптовалютам
/// </summary>
public interface ICryptocurrencyRepository
{
    /// <summary>
    /// Получить список всех добавленных криптовалют
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Список криптовалют. </returns>
    IAsyncEnumerable<Cryptocurrency> GetAllAvailable(Specification specification);

    /// <summary>
    /// Получить криптовалюту по идентификатору
    /// </summary>
    /// <param name="id"> Идентификатор монеты </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Криптовалюта </returns>
    Task<Cryptocurrency?> GetAvailableById(Guid id, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Проверить наличие криптовалюты по полному и/или короткому названию
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="fullName"> Полное название криптовалюты. </param>
    /// <param name="shortName"> Сокращённое название криптовалюты. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns>
    /// <see langword="true"/>, если криптовалюта была найдена хотя бы по одному параметру, иначе <see langword="false"/>
    /// </returns>
    Task<bool> Exists(Guid userId, string fullName, string shortName, CancellationToken cancellationToken);

    /// <summary>
    /// Добавить криптовалюту
    /// </summary>
    /// <param name="cryptocurrency"> Криптовалюта </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task Add(Cryptocurrency cryptocurrency);

    /// <summary>
    /// Удалить криптовалюту
    /// </summary>
    /// <param name="id"> Идентификатор криптовалюты. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task Remove(Guid id, Guid userId);
}
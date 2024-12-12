namespace MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;

using Wallet = Core.Mining.Wallet;

/// <summary>
/// Интерфейс репозитория для доступа к данным кошельков
/// </summary>
public interface IWalletRepository
{
    /// <summary>
    /// Получить список кошельков
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Список кошельков </returns>
    IAsyncEnumerable<Wallet> GetAllAvailable(Specification specification);

    /// <summary>
    /// Получить кошелёк по уникальному идентификатору
    /// </summary>
    /// <param name="id"> Уникальный идентификатор </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Кошелёк </returns>
    Task<Wallet?> GetAvailableById(Guid id, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получить признак существования кошелька c переданным названием.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="name"> Название. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns>
    /// <see langword="true"/>, если существует, иначе <see langword="false"/>.
    /// </returns>
    Task<bool> ExistsWithName(Guid userId, string name, CancellationToken cancellationToken);

    /// <summary>
    /// Получить признак существования кошелька с переданным адресом.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="address"> Адрес. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns>
    /// <see langword="true"/>, если существует, иначе <see langword="false"/>.
    /// </returns>
    Task<bool> ExistsWithAddress(Guid userId, string address, CancellationToken cancellationToken);

    /// <summary>
    /// Добавить кошелёк
    /// </summary>
    /// <param name="wallet"> Кошелёк </param>
    /// <returns> Идентификатор </returns>
    Task Add(Wallet wallet);

    /// <summary>
    /// Обновить данные о кошельке
    /// </summary>
    /// <param name="wallet"> Кошелёк с новыми данными </param>
    Task Update(Wallet wallet);

    /// <summary>
    /// Удалить кошелёк
    /// </summary>
    /// <param name="id"> Идентификатор кошелька. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    Task Remove(Guid id, Guid userId);
}
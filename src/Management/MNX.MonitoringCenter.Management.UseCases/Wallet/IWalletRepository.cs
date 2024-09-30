namespace MNX.MonitoringCenter.Management.UseCases.Wallet;

using Wallet = Core.Wallet;

/// <summary>
/// Интерфейс репозитория для доступа к данным кошельков
/// </summary>
public interface IWalletRepository
{
    /// <summary>
    /// Получить список кошельков
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Список кошельков </returns>
    IAsyncEnumerable<Wallet> GetAllAvailable(Guid userId);

    /// <summary>
    /// Получить кошелёк по уникальному идентификатору
    /// </summary>
    /// <param name="Id"> Уникальный идентификатор </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Кошелёк </returns>
    Task<Wallet?> GetAvailableById(Guid Id, Guid userId);

    /// <summary>
    /// Получить признак существования кошелька c переданным названием.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="name"> Название. </param>
    /// <returns>
    /// <see langword="true"/>, если существует, иначе <see langword="false"/>.
    /// </returns>
    Task<bool> ExistsWithName(Guid userId, string name);

    /// <summary>
    /// Получить признак существования кошелька с переданным адресом.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="address"> Адрес. </param>
    /// <returns>
    /// <see langword="true"/>, если существует, иначе <see langword="false"/>.
    /// </returns>
    Task<bool> ExistsWithAddress(Guid userId, string address);

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
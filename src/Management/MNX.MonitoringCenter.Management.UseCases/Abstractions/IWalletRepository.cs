using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Abstractions;

/// <summary>
/// Интерфейс репозитория для доступа к данным кошельков
/// </summary>
public interface IWalletRepository
{
    /// <summary>
    /// Получить список кошельков
    /// </summary>
    /// <returns> Список кошельков </returns>
    IAsyncEnumerable<Wallet> GetAll();

    /// <summary>
    /// Получить кошлёк по уникальному идентификатору
    /// </summary>
    /// <param name="Id"> Уникальный идентификатор </param>
    /// <returns> Кошелёк </returns>
    Task<Wallet?> GetById(Guid Id);

    /// <summary>
    /// Получить признак существования кошелька
    /// </summary>
    /// <param name="name"> Имя </param>
    /// <param name="address"> Адрес </param>
    /// <returns>
    /// <see langword="true"/>, если удаётся найти кошелёк хотя бы по одному параметру, иначе <see langword="false"/>
    /// </returns>
    Task<bool> Exists(string name, string address);

    /// <summary>
    /// Получить признак существования кошелька не брав в рассмотрение кошелёк с переданным идентификатором
    /// </summary>
    /// <param name="name"> Имя </param>
    /// <param name="address"> Адрес </param>
    /// <param name="exceptId"> Идентификатор кошелька, который не будет учитан </param>
    /// <returns>
    /// <see langword="true"/>, если удаётся найти кошелёк хотя бы по одному параметру, иначе <see langword="false"/>
    /// </returns>
    Task<bool> Exists(string name, string address, Guid exceptId);

    /// <summary>
    /// Добавить кошелёк
    /// </summary>
    /// <param name="wallet"> Кошелёк </param>
    /// <returns> Идентификатор </returns>
    Task<Guid> Add(Wallet wallet);

    /// <summary>
    /// Обновить данные о кошельке
    /// </summary>
    /// <param name="wallet"> Кошелёк с новыми данными </param>
    Task Update(Wallet wallet);

    /// <summary>
    /// Удалить кошелёк
    /// </summary>
    /// <param name="wallet"> Кошелёк </param>
    Task Remove(Wallet wallet);
}
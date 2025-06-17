using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;

namespace MNX.MonitoringCenter.Inventory.UseCases;

/// <summary>
/// Репозиторий для доступа к ригам.
/// </summary>
public interface IRigRepository
{
    /// <summary>
    /// Получить список ригов.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Асинхронный поток ригов. </returns>
    IAsyncEnumerable<RigDetails> GetRigs(InventorySpecification specification);

    /// <summary>
    /// Получить список идентификаторов ригов по совпадению установленных майнеров.
    /// </summary>
    /// <param name="minerName"> Наименование майнера. </param>
    /// <param name="minerVersion"> Версия майнера. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Массив идентификаторов ригов с совпадающими майнерами. </returns>
    Task<Guid[]> GetRigIdsByMinerCoincidence(string minerName,
                                             string minerVersion,
                                             Guid userId,
                                             CancellationToken cancellationToken);

    /// <summary>
    /// Получить список идентификаторов ригов, у которых нет заданного майнера.
    /// </summary>
    /// <param name="rigIds"> Идентификаторы ригов. </param>
    /// <param name="userId"> ИДентификатор пользователя. </param>
    /// <param name="minerName"> Наименование майнера. </param>
    /// <param name="minerVersion"> Версия майнера. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Массив идентификаторов ригов без майнера. </returns>
    Task<Guid[]> GetRigIdsWithoutMiner(Guid[] rigIds,
                                       Guid userId,
                                       string minerName,
                                       string minerVersion,
                                       CancellationToken cancellationToken);

    /// <summary>
    /// Получить признак существования рига.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns>
    /// <see cref="true"/>, если риг существует, иначе <see cref="false"/>.
    /// </returns>
    Task<bool> Exists(Guid rigId, CancellationToken cancellationToken);

    /// <summary>
    /// Добавить риг.
    /// </summary>
    /// <param name="rig"> Риг. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task Add(Rig rig, CancellationToken cancellationToken);

    /// <summary>
    /// Сохранить инвентаризацию рига.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="createdDate"> Дата проведения инвентаризации. </param>
    /// <param name="inventory"> Результат инвентаризации. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Идентификатор инвентаризации. </returns>
    Task<long> SaveInventory(Guid rigId, DateTimeOffset createdDate,
                             RigInventoryModel inventory, CancellationToken cancellationToken);

    /// <summary>
    /// Установить дату и время окончания действия инвентаризации.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task SetInventoryExpirationDate(Guid rigId, CancellationToken cancellationToken);
}

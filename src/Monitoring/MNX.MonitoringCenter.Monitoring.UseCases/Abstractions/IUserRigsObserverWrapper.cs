using MNX.MonitoringCenter.Monitoring.Contracts.Models;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

/// <summary>
/// Оболочка над наблюдателями за ригами.
/// </summary>
public interface IUserRigsObserverWrapper : IDisposable
{
    /// <summary>
    /// Добавить нового подписчика.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    Task AddNewSubscriber(long userId, string subscriberId);

    /// <summary>
    /// Удалить подписчика.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    Task RemoveSubscriber(long userId, string subscriberId);

    /// <summary>
    /// Задать отслеживаемую монету.
    /// </summary>
    /// <param name="coin"> Монета. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    void SetObservableCoin(string coin, long userId, string subscriberId);

    /// <summary>
    /// Получены динамические данные с ригов.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="data"> Данные. </param>
    Task GotDynamicData(long userId, List<RigDynamicData> data);

    /// <summary>
    /// Получено состояние ригов.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <param name="rigs"> Состояние ригов. </param>
    Task GotRigsState(long userId, string subscriberId, List<RigState> rigs);
}

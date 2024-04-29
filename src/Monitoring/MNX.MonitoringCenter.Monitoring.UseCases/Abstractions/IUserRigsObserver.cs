using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

/// <summary>
/// Наблюдатель за ригами пользователя.
/// </summary>
public interface IUserRigsObserver : IDisposable
{
    /// <summary>
    /// Подписаться.
    /// </summary>
    /// <remarks> Количество подписчиков. </remarks>
    Task<long> Subscribe(string subscriberId);

    /// <summary>
    /// Отписаться.
    /// </summary>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <returns> Количество подписчиков. </returns>
    Task<long> Unsubscribe(string subscriberId);

    /// <summary>
    /// Задать отслеживаемую монету.
    /// </summary>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <param name="coin"> Монета. </param>
    Task SetObservableCoin(string subscriberId, string coin);

    /// <summary>
    /// Получены динамические данные с ригов.
    /// </summary>
    /// <param name="rigDynamicData"> Динамические данные с ригов. </param>
    void GotDynamicData(List<RigDynamicData> rigDynamicData);

    /// <summary>
    /// Получено состояние ригов.
    /// </summary>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <param name="rigs"> Состояния ригов. </param>
    Task GotRigsState(string subscriberId, List<RigState> rigs);
}

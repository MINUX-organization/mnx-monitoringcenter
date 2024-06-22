using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;
using MNX.MonitoringCenter.Monitoring.Core;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

/// <summary>
/// Наблюдатель за ригами пользователя.
/// </summary>
public interface IUserRigsObserver : IDisposable
{
    /// <summary>
    /// Подписаться.
    /// </summary>
    /// <remarks>
    /// Сразу после подписки подписчик получит информацию о состоянии ригов.
    /// В случае изменения состояния подписчик будет уведомлён об этом.
    /// Допускается отдельная возможность подписки на поток динамических данных.
    /// </remarks>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <param name="subscribeToDynamicDataStream"> Признак подписки на поток динамических данных. </param>
    /// <returns> Количество подписчиков. </returns>
    Task<long> Subscribe(string subscriberId, bool subscribeToDynamicDataStream);

    /// <summary>
    /// Отписаться.
    /// </summary>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <returns> Количество подписчиков. </returns>
    Task<long> Unsubscribe(string subscriberId);

    /// <summary>
    /// Задать отслеживаемую монету.
    /// </summary>
    /// <remarks>
    /// Этот метод позволяет подписаться на изменения скорости хеширования конкретной монеты.
    /// </remarks>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <param name="coin"> Монета. </param>
    Task SetObservableCoin(string subscriberId, string coin);

    /// <summary>
    /// Задать строку поиска.
    /// </summary>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <param name="searchString">  Строка поиска. </param>
    void SetSearchString(string subscriberId, string searchString);

    /// <summary>
    /// Задать разгон для видеокарты.
    /// </summary>
    /// <param name="cardId"> Идентификатор видеокарты. </param>
    /// <param name="overclocking"> Разгон. </param>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    Task SetOverclocking(string subscriberId, Guid cardId, OverclockingModel overclocking);

    /// <summary>
    /// Получен разгон для видеокарты.
    /// </summary>
    /// <param name="cardId"> Идентификатор видеокарты. </param>
    /// <param name="overclocking"> Разгон. </param>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    Task GotOverclockingSettingSuccess(string subscriberId, Guid cardId, OverclockingModel overclocking);

    /// <summary>
    /// Получено сообщение о неудачной установке разгона.
    /// </summary>
    /// <param name="cardId"> Идентификатор видеокарты. </param>
    /// <param name="message"> Сообщение об ошибке. </param>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    Task GotOverclockingSettingFail(string subscriberId, Guid cardId, string message);

    /// <summary>
    /// Получены динамические данные с ригов.
    /// </summary>
    /// <param name="rigDynamicData"> Динамические данные с ригов. </param>
    void GotDynamicData(List<RigDynamicData> rigDynamicData);

    /// <summary>
    /// Получено состояние ригов.
    /// </summary>
    /// <param name="subscriberId"> Идентификатор подписчика, который запросил состояние. </param>
    /// <param name="rigs"> Состояния ригов. </param>
    Task GotRigsState(string subscriberId, List<RigState> rigs);
}

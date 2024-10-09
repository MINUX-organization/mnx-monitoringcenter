using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Traffic.Observers;

/// <summary>
/// Тип подписки.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SubscriptionType
{
    #region Hardware
    /// <summary>
    /// Основные аппаратные показатели рига.
    /// </summary>
    GeneralHardwareRigsIndicators,

    /// <summary>
    /// Аппаратные показатели процессоров.
    /// </summary>
    CpusHardwareIndicators,

    /// <summary>
    /// Аппаратные показатели видеокарт.
    /// </summary>
    GpusHardwareIndicators,

    /// <summary>
    /// Общая мощность.
    /// </summary>
    TotalPower,
    #endregion

    #region
    /// <summary>
    /// Основные показатели майнинга рига.
    /// </summary>
    GeneralMiningRigsIndicators,

    /// <summary>
    /// Показатели майнинга процессоров.
    /// </summary>
    CpusMiningIndicators,

    /// <summary>
    /// Показатели майнинга видеокарт.
    /// </summary>
    GpusMiningIndicators,

    /// <summary>
    /// Обобщённые решения майнинга.
    /// </summary>
    TotalShares,

    /// <summary>
    /// Общая скорость хеширования.
    /// </summary>
    TotalHashRate,

    /// <summary>
    /// Обобщённая статистика майнинга по монетам.
    /// </summary>
    TotalCoinsStatistics
    #endregion
}

/// <summary>
/// Расширения для типа подписки.
/// </summary>
public static class SubscriptionTypeExtensions
{
    /// <summary>
    /// Получить признак присутствия подписки на поток аппаратных показателей.
    /// </summary>
    /// <param name="subscription"> Тип подписки. </param>
    /// <returns>
    /// <see langword="true"/>, если подписка присутствует, иначе <see langword="false"/>.
    /// </returns>
    public static bool IsHardwareSubscription(this SubscriptionType subscription)
    {
        return subscription == SubscriptionType.GeneralHardwareRigsIndicators ||
               subscription == SubscriptionType.CpusHardwareIndicators ||
               subscription == SubscriptionType.GpusHardwareIndicators ||
               subscription == SubscriptionType.TotalPower;
    }

    /// <summary>
    /// Получить признак присутствия подписки на поток показателей майнинга.
    /// </summary>
    /// <param name="subscription"> Тип подписки. </param>
    /// <returns>
    /// <see langword="true"/>, если подписка присутствует, иначе <see langword="false"/>.
    /// </returns>
    public static bool IsMiningSubscription(this SubscriptionType subscription)
    {
        return subscription == SubscriptionType.GeneralMiningRigsIndicators ||
               subscription == SubscriptionType.CpusMiningIndicators ||
               subscription == SubscriptionType.GpusMiningIndicators ||
               subscription == SubscriptionType.TotalShares ||
               subscription == SubscriptionType.TotalCoinsStatistics ||
               subscription == SubscriptionType.TotalHashRate;
    }
}

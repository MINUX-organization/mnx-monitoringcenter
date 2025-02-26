namespace MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

/// <summary>
/// Метрики майнинга.
/// </summary>
public class MiningMetrics
{
    /// <summary>
    /// Скорость хеширования.
    /// </summary>
    public int HashRate { get; set; }

    /// <summary>
    /// Решения.
    /// </summary>
    public required SharesModel Shares { get; set; }

    /// <summary>
    /// Оператор сложения.
    /// </summary>
    /// <param name="first"> Первое слагаемое. </param>
    /// <param name="second"> Второе слагаемое. </param>
    /// <returns> Сумма метрик. </returns>
    public static MiningMetrics operator +(MiningMetrics first, MiningMetrics second)
    {
        return new MiningMetrics
        {
            HashRate = first.HashRate + second.HashRate,
            Shares = first.Shares + second.Shares
        };
    }
}

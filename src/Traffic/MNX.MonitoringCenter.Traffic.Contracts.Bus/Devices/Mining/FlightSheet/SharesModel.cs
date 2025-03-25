namespace MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

/// <summary>
/// Решения.
/// </summary>
public class SharesModel
{
    /// <summary>
    /// Принятые решения.
    /// </summary>
    public int Accepted { get; set; }

    /// <summary>
    /// Отклонённые решения.
    /// </summary>
    public int Rejected { get; set; }

    /// <summary>
    /// Оператор сложения решений.
    /// </summary>
    /// <param name="first"> Первое слагаемое. </param>
    /// <param name="second"> Второе слагаемое. </param>
    /// <returns> Сумма решений. </returns>
    public static SharesModel operator +(SharesModel first, SharesModel second)
    {
        return new SharesModel()
        {
            Accepted = first.Accepted + second.Accepted,
            Rejected = first.Rejected + second.Rejected
        };
    }
}

namespace MNX.MonitoringCenter.Monitoring.Contracts.Models;

/// <summary>
/// Модель с Shares
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
    /// Сложить два объекта с шерами.
    /// </summary>
    /// <param name="first"> Первый объект. </param>
    /// <param name="second"> Новый объект. </param>
    /// <returns> Сумма шеров. </returns>
    public static SharesModel operator + (SharesModel first, SharesModel second)
    {
        return new SharesModel()
        {
            Accepted = first.Accepted + second.Accepted,
            Rejected = first.Rejected + second.Rejected
        };
    }
}

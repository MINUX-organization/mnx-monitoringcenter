using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.InternetAdapter;

/// <summary>
/// Информация об интернет адаптере.
/// </summary>
[ComplexType]
public class InternetAdapterInformation
{
    /// <summary>
    /// Производитель.
    /// </summary>
    public string Manufacturer { get; set; }

    /// <summary>
    /// Модель.
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Серийный номер.
    /// </summary>
    public string SerialNumber { get; set; }

    /// <summary>
    /// Код продавца.
    /// </summary>
    public string VendorCode { get; set; }

    /// <summary>
    /// Информация о BUS.
    /// </summary>
    public string BusInfo { get; set; }

    /// <summary>
    /// Логическое имя.
    /// </summary>
    public string LogicalName { get; set; }

    /// <summary>
    /// MAC адрес.
    /// </summary>
    public string Mac { get; set; }
}

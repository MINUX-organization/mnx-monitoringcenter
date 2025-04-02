namespace MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

/// <summary>
/// Перечисление состояния подтверждения применения полетного листа.
/// </summary>
public enum FlightSheetConfirmationState
{
    /// <summary>
    /// Не подтверждено применение настроек полетного листа.
    /// </summary>
    Unconfirmed,

    /// <summary>
    /// Успешное применение настроек полетного листа.
    /// </summary>
    Successfully,

    /// <summary>
    /// Ошибка применения настроек полетного листа.
    /// </summary>
    Error,
}

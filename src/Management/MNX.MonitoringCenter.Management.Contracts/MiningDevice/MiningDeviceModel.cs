using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Contracts.MiningDevice;

/// <summary>
/// Модуль майнинга устройства.
/// </summary>
public class MiningDeviceModel
{
    /// <summary>
    /// Идентификатор майнинг устройства.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Производитель.
    /// </summary>
    public required string Manufacturer { get; init; }

    /// <summary>
    /// Модель.
    /// </summary>
    public required string Model { get; init; }

    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get => $"{Manufacturer} {Model}"; }

    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    public Guid RigId { get; init; }

    /// <summary>
    /// Тип майнинг устройства.
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Идентификатор полётного листа.
    /// </summary>
    public Guid? FlightSheetId { get; init; }

    /// <summary>
    /// Название полётного листа.
    /// </summary>
    public string? FlightSheetName { get; init; }

    /// <summary>
    /// Наименование пресета.
    /// </summary>
    public string? PresetName { get; init; }

    /// <summary>
    /// Признак подтверждения текущего значения полётного листа.
    /// </summary>
    /// <returns>
    /// <see cref="FlightSheetConfirmationState.Unconfirmed"/>, если  значение не подтверждено ригом,
    /// <see cref="FlightSheetConfirmationState.Successfully"/>, если значение подтверждено ригом,
    /// <see cref="FlightSheetConfirmationState.Error"/>, если произошла ошибка при подтверждении..
    /// </returns>
    public FlightSheetConfirmationState FlightSheetConfirmationState { get; init; }

    /// <summary>
    /// Название майнера.
    /// </summary>
    public string? MinerName { get; init; }

    /// <summary>
    /// Версия майнера.
    /// </summary>
    public string? MinerVersion { get; init; }

    /// <summary>
    /// Признак нахождения устройства в сети.
    /// </summary>
    public bool IsOnline { get; init; }
}

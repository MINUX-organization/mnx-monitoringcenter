using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetMiningDevices;

/// <summary>
/// Майнинг устройство.
/// </summary>
public class MiningDevice
{
    /// <summary>
    /// Идентификатор майнинг устройства.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Производитель майнинг устройства.
    /// </summary>
    public required string Manufacturer { get; init; }

    /// <summary>
    /// Модель майнинг устройства.
    /// </summary>
    public required string Model { get; init; }

    /// <summary>
    /// Название рига.
    /// </summary>
    public string? RigName { get; init; }

    /// <summary>
    /// Тип майнинг устройства.
    /// </summary>
    public required string Type { get; init; }

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
    /// <see cref="FlightSheetConfirmationState.Error"/>, если произошла ошибка при подтверждении.
    /// </returns>
    public FlightSheetConfirmationState FlightSheetConfirmationState { get; init; }

    /// <summary>
    /// Название майнера.
    /// </summary>
    public string? MinerName { get; init; }

    /// <summary>
    /// Идентификатор шины PCI.
    /// </summary>
    public string? PciBus { get; set; }

    /// <summary>
    /// Признак нахождения устройства в сети.
    /// </summary>
    public bool IsOnline { get; init; }
}

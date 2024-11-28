using MNX.MonitoringCenter.Management.Core.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto.Target;

/// <summary>
/// DTO базового таргета полётного листа.
/// </summary>
public abstract class BaseFlightSheetTargetDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Идентификатор полётного листа.
    /// </summary>
    public Guid FlightSheetId { get; set; }

    /// <summary>
    /// Тип устройства, для которого предназначен таргет.
    /// </summary>
    public MiningDeviceType DeviceType { get; set; }

    /// <summary>
    /// Список конфигов монет для майнинга.
    /// </summary>
    public List<MiningCoinConfig> CoinConfigs { get; set; } = new(3);

    /// <summary>
    /// Строка дополнительных аргументов для майнера.
    /// </summary>
    public string? AdditionalArguments { get; set; }

    /// <summary>
    /// Строка конфигурации формата Json.
    /// </summary>
    public string? ConfigFileContent { get; set; }

    /// <summary>
    /// Идентификатор майнера.
    /// </summary>
    public Guid MinerId { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public Core.Miner.Miner? Miner { get; set; }

    /// <summary>
    /// Создать майнинг конфиг.
    /// </summary>
    public abstract BaseMiningConfig CreateMiningConfig();
}

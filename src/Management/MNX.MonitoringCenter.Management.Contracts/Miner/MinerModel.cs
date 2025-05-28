using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Contracts.Miner;

/// <summary>
/// Контракт модели майнера.
/// </summary>
public class MinerModel
{
    /// <summary>
    /// Идентификатор майнера.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Наименование майнера.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Версия майнера.
    /// </summary>
    public required string Version { get; set; }

    /// <summary>
    /// Поддерживаемые типы устройств.
    /// </summary>
    public DeviceTypeManufacturerCombination SupportedDevices { get; set; }

    /// <summary>
    /// Список алгоритмов.
    /// </summary>
    public List<MinerAlgorithmModel> Algorithms { get; set; } = [];

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    /// <remarks>
    /// Если null, то сущность является интегрированной,
    /// иначе - пользовательской.
    /// </remarks>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Ссылка установки майнера.
    /// </summary>
    public required string InstallationUrl { get; set; }

    /// <summary>
    /// Шаблон блока пула.
    /// </summary>
    public string? PoolTemplate { get; set; }

    /// <summary>
    /// Адрес кошелька и имя воркера для идентификации на пуле.
    /// </summary>
    public string? WalletWorkerTemplate { get; set; }

    /// <summary>
    /// Режим майнинга.
    /// </summary>
    public MiningModeEnum MiningMode { get; set; }
}
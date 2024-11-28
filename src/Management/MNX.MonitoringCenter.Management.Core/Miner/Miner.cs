using MNX.MonitoringCenter.Management.Core.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Core.Miner;

/// <summary>
/// Майнер.
/// </summary>
public class Miner : IEquatable<Miner>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Версия.
    /// </summary>
    public required string Version { get; set; }

    /// <summary>
    /// Поддерживаемые комбинации типа устройств и производителя.
    /// </summary>
    public DeviceTypeManufacturerCombination SupportedDevices { get; set; }

    /// <summary>
    /// Режим майнинга монет.
    /// </summary>
    public MiningModeEnum MiningMode { get; set; } = MiningModeEnum.Single;

    /// <summary>
    /// Получить признак поддержки майнером переданной конфигурации.
    /// </summary>
    /// <param name="config"> Конфигурация для майнинга. </param>
    /// <returns>
    /// <see langword="true"/>, если майнер поддерживает переданную конфигурацию,
    /// иначе <see langword="false"/>.
    /// </returns>
    public bool IsSupportConfigs(BaseMiningConfig config, out IReadOnlyCollection<string> errors)
    {
        var e = new List<string>();

        if (config.CoinConfigs.Count > (int)MiningMode)
        {
            e.Add($"Invalid number of coins was passed for mining mode " +
                  $"{MiningMode}: {config.CoinConfigs.Count}.");

            errors = e;
            return false;
        }

        if (!SupportedDevices.IsSupportDeviceType(config.DeviceType))
        {
            e.Add("Device type in config is not supported by the miner.");
            errors = e;
            return false;
        }

        errors = e;
        return true;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is Miner miner)
        {
            return Equals(miner);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(Miner? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Name == other.Name && Version == other.Version;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Version);
    }
}
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Core.Mining.Miner;

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
    public required string Version { get; init; }

    /// <summary>
    /// Поддерживаемые алгоритмы.
    /// </summary>
    public List<MinerAlgorithm> SupportedAlgorithms { get; init; } = new(0);

    /// <summary>
    /// Поддерживаемые комбинации типа устройств и производителя.
    /// </summary>
    public DeviceTypeManufacturerCombination SupportedDevices { get; init; }

    /// <summary>
    /// Режим майнинга монет.
    /// </summary>
    public MiningModeEnum MiningMode { get; init; } = MiningModeEnum.Single;

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

        if (!SupportedAlgorithms.Any(x 
            => config.CoinConfigs.Any(y => x.AlgorithmId == y.Pool!.Cryptocurrency!.AlgorithmId)))
        {
            e.Add("Algorithm is not supported by miner.");
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

    /// <summary>
    /// Получить признак поддержки устройства.
    /// </summary>
    /// <param name="device"> Устройство. </param>
    /// <returns> Признак поддержки устройства. </returns>
    public bool IsDeviceSupport(MiningDevice.MiningDevice device)
    {
        return SupportedDevices.IsSupportDeviceType(device.Type) &&
               SupportedDevices.IsSupportDeviceManufacturer(device.Manufacturer);
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
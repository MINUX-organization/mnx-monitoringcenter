using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;

/// <summary>
/// Базовая конфигурация для майнинга.
/// </summary>
public abstract class BaseMiningConfig : IEquatable<BaseMiningConfig>
{
    /// <summary>
    /// Тип устройства, для которого предназначен конфиг.
    /// </summary>
    public abstract MiningDeviceType DeviceType { get; }

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
    /// Получить признак валидности конфига для майнинга.
    /// </summary>
    /// <param name="errors"> Ошибки. </param>
    /// <returns> Признак валидности. </returns>
    public bool IsValid(out IReadOnlyCollection<string> errors)
    {
        bool isValid = true;
        var e = new List<string>();

        if (CoinConfigs.Count == 0)
        {
            e.Add("Configs are required!");
            isValid = false;
        }

        if (DeviceType == MiningDeviceType.GPU && CoinConfigs.Count > 3 ||
            DeviceType == MiningDeviceType.CPU && CoinConfigs.Count > 1)
        {
            e.Add("The number of configs for a flight sheet " +
                  "should not exceed 3 for a GPU and not exceed 1 for a CPU!");
        }

        var coinIds = new List<Guid>(3);
        foreach (var coinConfig in CoinConfigs)
        {
            if (!coinConfig.IsValid(out IReadOnlyCollection<string> coinConfigErrors))
            {
                e.AddRange(coinConfigErrors);
                isValid = false;
            }

            if (coinConfig.Pool is null)
                continue;

            if (coinIds.Contains(coinConfig.Pool.CryptocurrencyId))
            {
                e.Add($"Cannot use the same cryptocurrency" +
                      $"({coinConfig.Pool.CryptocurrencyId}) in different target configs.");
                isValid = false;
            }

            coinIds.Add(coinConfig.Pool.CryptocurrencyId);
        }

        errors = e;
        return isValid;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is BaseMiningConfig flightSheet)
        {
            return Equals(flightSheet);
        }

        return false;
    }

    /// <inheritdoc/>
    public virtual bool Equals(BaseMiningConfig? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return DeviceType == other.DeviceType &&
               AdditionalArguments == other.AdditionalArguments &&
               ConfigFileContent == other.ConfigFileContent &&
               CoinConfigs.SequenceEqual(other.CoinConfigs);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hashCode = 0;

        foreach (var config in CoinConfigs)
        {
            hashCode += config.GetHashCode();
        }

        return HashCode.Combine(hashCode, DeviceType, AdditionalArguments, ConfigFileContent);
    }
}

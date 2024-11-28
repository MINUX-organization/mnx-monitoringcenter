using MNX.MonitoringCenter.Management.Core.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

/// <summary>
/// Таргет полётного листа.
/// </summary>
/// <remarks>
/// Содержит майнер и конфиг к нему для запуска майнинга на определённом типе устройств.
/// </remarks>
public class FlightSheetTarget : IEquatable<FlightSheetTarget>
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
    public MiningDeviceType DeviceType { get => MiningConfig.DeviceType; }

    /// <summary>
    /// Конфигурация для майнинга.
    /// </summary>
    public required BaseMiningConfig MiningConfig { get; set; }

    /// <summary>
    /// Идентификатор майнера.
    /// </summary>
    public Guid MinerId { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public Miner.Miner? Miner { get; set; }

    /// <summary>
    /// Получить признак валидности таргета полётного листа.
    /// </summary>
    /// <param name="errors"> Ошибки. </param>
    /// <returns> Признак валидности. </returns>
    public bool IsValid(out IReadOnlyCollection<string> errors)
    {
        bool isValid = true;
        var e = new List<string>();

        if (!MiningConfig.IsValid(out IReadOnlyCollection<string> configErrors))
        {
            e.AddRange(configErrors);
            isValid = false;
        }

        if (Miner is null)
        {
            e.Add("Miner is required!");
            isValid = false;
        }

        if (! Miner!.IsSupportConfigs(MiningConfig, out IReadOnlyCollection<string> minerErrors))
        {
            e.AddRange(minerErrors);
            isValid = false;
        }

        errors = e;
        return isValid;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is FlightSheetTarget flightSheet)
        {
            return Equals(flightSheet);
        }

        return false;
    }

    /// <inheritdoc/>
    public virtual bool Equals(FlightSheetTarget? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return MinerId == other.MinerId &&
               MiningConfig.Equals(other.MiningConfig);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(MinerId, MiningConfig);
    }
}

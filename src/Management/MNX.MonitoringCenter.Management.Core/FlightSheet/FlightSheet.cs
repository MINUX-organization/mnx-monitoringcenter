using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Core.FlightSheet;

/// <summary>
/// Полётный лист.
/// </summary>
public class FlightSheet : IEquatable<FlightSheet>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Таргеты.
    /// </summary>
    public List<FlightSheetTarget> Targets { get; set; } = new(0);

    /// <summary>
    /// Получить признак валидности полётного листа.
    /// </summary>
    /// <param name="errors"> Ошибки. </param>
    /// <returns> Признак валидности. </returns>
    public bool IsValid(out IReadOnlyCollection<string> errors)
    {
        bool isValid = true;
        var e = new List<string>();

        if (Targets.Count == 0)
        {
            e.Add("Targets is required");
            isValid = false;
        }

        if (Targets.Count(x => x.DeviceType == MiningDeviceType.CPU) > 1)
        {
            e.Add("The number of CPU targets should not exceed 1");
            isValid = false;
        }

        if (Targets.Count(x => x.DeviceType == MiningDeviceType.GPU) > 1)
        {
            e.Add("The number of GPU targets should not exceed 1");
            isValid = false;
        }

        foreach (var target in Targets)
        {
            if (!target.IsValid(out IReadOnlyCollection<string> targetErrors))
            {
                e.AddRange(targetErrors);
                isValid = false;
            }
        }

        errors = e;
        return isValid;
    }

    /// <summary>
    /// Получить признак поддержки устройства.
    /// </summary>
    /// <param name="device"> Устройство. </param>
    /// <returns> Признак поддержки устройства. </returns>
    public bool IsDeviceSupport(MiningDevice.MiningDevice device)
    {
        return Targets.Any(target => target.MiningConfig.DeviceType == device.Type);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is FlightSheet flightSheet)
        {
            return Equals(flightSheet);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(FlightSheet? other)
    {
        if (other is null) return false;

        if (ReferenceEquals(this, other)) return true;

        return Name == other.Name &&
               UserId == other.UserId &&
               Targets.SequenceEqual(other.Targets);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int targetsHash = 0;

        foreach (var target in Targets)
        {
            targetsHash += target.GetHashCode();
        }

        return HashCode.Combine(Name, UserId, targetsHash);
    }
}

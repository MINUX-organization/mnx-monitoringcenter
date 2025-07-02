namespace MNX.MonitoringCenter.Inventory.Contracts.Devices;

/// <summary>
/// Ограничения параметров разгона типа <see cref="int"/>.
/// </summary>
public record IntegerTypeRestrictions
{
    public int? Minimal { get; init; }

    public int? Maximal { get; init; }

    public bool? IsWritable { get; init; }

    public int? Default { get; init; }
}

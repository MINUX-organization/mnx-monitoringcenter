namespace MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

/// <summary>
/// Идентификатор рига.
/// </summary>
public readonly record struct RigId
{
    public Guid Value { get; }

    public RigId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Rig id could not be empty" , nameof(value));

        Value = value;
    }

    public static implicit operator Guid(RigId id) => id.Value;

    public static explicit operator RigId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}

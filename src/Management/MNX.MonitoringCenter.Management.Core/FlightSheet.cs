namespace MNX.MonitoringCenter.Management.Core;

/// <summary>
/// Полётный лист ( конфигурация для воркера )
/// </summary>
public class FlightSheet : IEquatable<FlightSheet> { 
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Miner Miner { get; set; }

    public string Cryptocurrency { get; set; }

    public string WalletAddress { get; set; }

    public Pool Pool { get; set; }

    public Guid UserId {  get; set; }

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
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Miner.Equals(other.Miner) &&
               Cryptocurrency == other.Cryptocurrency &&
               WalletAddress == other.WalletAddress &&
               Pool.Equals(other.Pool);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Miner, Cryptocurrency,  WalletAddress, Pool);
    }
}
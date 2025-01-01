
namespace MNX.MonitoringCenter.Management.Contracts;

public class MiningCombinations
{
    public string? FlightSheet { get; init; }

    public string? Miner { get; init; }

    public string? Coin { get; init; }

    public void Deconstruct(out string? flightSheet, out string? miner, out string? coin)
    {
        flightSheet = FlightSheet;
        miner = Miner;
        coin = Coin;
    }
}

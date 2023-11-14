using MINUX.Backend.Unit.Core.StaticData;

namespace MINUX.Backend.Unit.Core;

public class FlightSheet
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Miner Miner { get; set; }

    public string Cryptocurrency { get; set; }

    public string WalletAddress { get; set; }

    public Pool Pool { get; set; }
}
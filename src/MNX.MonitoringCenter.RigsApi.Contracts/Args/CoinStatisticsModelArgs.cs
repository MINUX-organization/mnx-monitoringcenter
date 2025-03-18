using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Args;

public class CoinStatisticsModelArgs
{
    public required IEnumerable<CoinStatistics> CoinStatistics { get; set; }

    public required Dictionary<Guid, string> CoinNames { get; set; }
}

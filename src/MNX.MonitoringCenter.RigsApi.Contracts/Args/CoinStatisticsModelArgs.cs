using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Args;

public class CoinStatisticsModelArgs
{
    public required IEnumerable<CoinStatistics> CoinStatistics { get; set; }

    public required Dictionary<Guid, string> CoinNames { get; set; }
}

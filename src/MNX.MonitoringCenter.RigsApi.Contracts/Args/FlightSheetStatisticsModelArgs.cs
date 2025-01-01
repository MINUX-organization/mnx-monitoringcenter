using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Args;

public class FlightSheetStatisticsModelArgs
{
    public FlightSheetStatistics? FlightSheetStatistics { get; set; }

    public required Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombinations> MiningCombinations { get; set; }
}

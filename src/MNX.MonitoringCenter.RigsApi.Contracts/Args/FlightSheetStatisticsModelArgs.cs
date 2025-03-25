using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Args;

public class FlightSheetStatisticsModelArgs
{
    public FlightSheetStatistics? FlightSheetStatistics { get; set; }

    public required Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombination> MiningCombinations { get; set; }
}

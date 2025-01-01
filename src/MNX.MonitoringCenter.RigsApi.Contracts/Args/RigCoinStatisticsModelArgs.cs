using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Args;

public class RigCoinStatisticsModelArgs
{
    public required IEnumerable<RigCoinStatistics> RigCoinStatistics { get; set; }

    public required Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombinations> MiningCombinations { get; set; }
}
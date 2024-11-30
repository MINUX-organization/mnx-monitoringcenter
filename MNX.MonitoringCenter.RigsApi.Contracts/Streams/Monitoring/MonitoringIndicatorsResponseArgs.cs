using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Streams.Monitoring;

public class MonitoringIndicatorsResponseArgs
{
    public IEnumerable<CoinStatistics> TotalCoinStatistics { get; set; }

    public SharesModel TotalShares { get; set; }

    public int TotalHashrate { get; set; }

    public int TotalPower { get; set; }

    public IEnumerable<RigDynamicMiningIndicators> RigsDynamicMiningIndicators { get; set; }

    public MiningCombinations MiningCombinations { get; set; }

    public MonitoringIndicatorsResponseArgs(
        IEnumerable<CoinStatistics> coinStatistics,
        SharesModel sharesModel,
        int totalPower,
        int totalHashrate,
        IEnumerable<RigDynamicMiningIndicators> rigsDynamicMiningIndicators,
        MiningCombinations miningCombinations)
    {
        TotalCoinStatistics = coinStatistics;
        TotalShares = sharesModel;
        TotalHashrate = totalHashrate;
        TotalPower = totalPower;
        RigsDynamicMiningIndicators = rigsDynamicMiningIndicators;
        MiningCombinations = miningCombinations;
    }
}
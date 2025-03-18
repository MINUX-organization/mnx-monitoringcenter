using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.FlightSheet;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Args;

public class MonitoringIndicatorsStreamResponseArgs
{
    public IEnumerable<CoinStatistics> TotalCoinStatistics { get; set; }

    public SharesModel TotalShares { get; set; }

    public int TotalHashrate { get; set; }

    public int TotalPower { get; set; }

    public IEnumerable<RigDynamicMiningIndicators> RigsDynamicMiningIndicators { get; set; }

    public IEnumerable<RigDynamicHardwareIndicators> RigDynamicHardwareIndicators { get; set; }

    public Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombinations> MiningCombinations { get; set; }

    public Dictionary<Guid, RigDetails> Rigs { get; set; }

    public MonitoringIndicatorsStreamResponseArgs(
        IEnumerable<CoinStatistics> coinStatistics,
        SharesModel sharesModel,
        int totalPower,
        int totalHashrate,
        IEnumerable<RigDynamicMiningIndicators> rigsDynamicMiningIndicators,
        IEnumerable<RigDynamicHardwareIndicators> rigDynamicHardwareIndicators,
        Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombinations> miningCombinations,
        Dictionary<Guid, RigDetails> rigs)
    {
        TotalCoinStatistics = coinStatistics;
        TotalShares = sharesModel;
        TotalHashrate = totalHashrate;
        TotalPower = totalPower;
        RigsDynamicMiningIndicators = rigsDynamicMiningIndicators;
        RigDynamicHardwareIndicators = rigDynamicHardwareIndicators;
        MiningCombinations = miningCombinations;
        Rigs = rigs;
    }
}
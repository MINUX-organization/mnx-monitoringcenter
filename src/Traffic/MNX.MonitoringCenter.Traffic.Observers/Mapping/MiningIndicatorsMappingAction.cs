using AutoMapper;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.FlightSheet;

namespace MNX.MonitoringCenter.Traffic.Observers.Mapping;

/// <summary>
/// Пост-действие маппинга показателей майнинга.
/// </summary>
public class MiningIndicatorsMappingAction :
    IMappingAction<CpuDynamicIndicators, CpuDynamicMiningIndicators>,
    IMappingAction<GpuDynamicIndicators, GpuDynamicMiningIndicators>
{
    private const string MINER_NAME_KEY = "MinerName";

    /// <inheritdoc/>
    public void Process(CpuDynamicIndicators source, CpuDynamicMiningIndicators destination, ResolutionContext context)
    {
        Process(destination, context);
    }

    /// <inheritdoc/>
    public void Process(GpuDynamicIndicators source, GpuDynamicMiningIndicators destination, ResolutionContext context)
    {
        Process(destination, context);

    }

    public static void Process(IDeviceDynamicMiningIndicators destination, ResolutionContext context)
    {
        var minerName = context.Items.GetValueOrDefault(MINER_NAME_KEY);

        if (minerName is not null)
        {
            destination.FlightSheet = new FlightSheetStatistics()
            {
                MinerName = (string)minerName
            };
        }
    }
}

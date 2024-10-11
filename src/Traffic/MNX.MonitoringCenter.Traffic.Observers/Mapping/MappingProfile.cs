using AutoMapper;
using MNX.MonitoringCenter.Traffic.Contracts.Bus;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Abstractions;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Network;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.Abstractions;

namespace MNX.MonitoringCenter.Traffic.Observers.Mapping;

/// <summary>
/// Конфигурация маппинга.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RigDynamicIndicators, RigDynamicHardwareIndicators>()
            .AfterMap((src, dest) =>
            {
                var devices = new List<IDeviceDynamicHardwareIndicators>(dest.Devices.Where(x => x is not null));
                dest.Devices.Clear();
                dest.Devices.AddRange(devices);
            });

        CreateMap<RigDynamicIndicators, RigDynamicMiningIndicators>()
            .AfterMap((src, dest) =>
            {
                var devices = new List<IDeviceDynamicMiningIndicators>(dest.Devices.Where(x => x is not null));
                dest.Devices.Clear();
                dest.Devices.AddRange(devices);
            });

        CreateMap<IDeviceDynamicIndicators, IDeviceDynamicHardwareIndicators>()
            .ConvertUsing(new DeviceDynamicIndicatorsConverter());
        CreateMap<IDeviceDynamicIndicators, IDeviceDynamicMiningIndicators>()
            .ConvertUsing(new DeviceDynamicIndicatorsConverter());

        CreateMap<CpuDynamicIndicators, CpuDynamicHardwareIndicators>();
        CreateMap<GpuDynamicIndicators, GpuDynamicHardwareIndicators>();
        CreateMap<NetworkAdapterDynamicIndicators, NetworkAdapterDynamicHardwareIndicators>();

        CreateMap<CpuDynamicIndicators, CpuDynamicMiningIndicators>();
        CreateMap<GpuDynamicIndicators, GpuDynamicMiningIndicators>();
    }
}

using AutoMapper;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Abstractions;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Network;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.Abstractions;

namespace MNX.MonitoringCenter.Traffic.Observers.Mapping;

/// <summary>
/// Конвертер динамических показателей устройства.
/// </summary>
public class DeviceDynamicIndicatorsConverter :
    ITypeConverter<IDeviceDynamicIndicators, IDeviceDynamicHardwareIndicators>,
    ITypeConverter<IDeviceDynamicIndicators, IDeviceDynamicMiningIndicators>
{
    /// <summary>
    /// Конвертировать динамические показатели устройств в динамические аппаратные показатели устройств.
    /// </summary>
    /// <param name="source"> Динамические показатели устройств. </param>
    /// <param name="destination"> Динамические аппаратные показатели устройств. </param>
    /// <param name="context"> Контекст. </param>
    /// <returns> Динамические аппаратные показатели устройств. </returns>
    public IDeviceDynamicHardwareIndicators Convert(IDeviceDynamicIndicators source,
                                                    IDeviceDynamicHardwareIndicators destination,
                                                    ResolutionContext context)
    {
        if (source is CpuDynamicIndicators cpuIndicators)
        {
            return context.Mapper.Map<CpuDynamicHardwareIndicators>(cpuIndicators);
        }
        else if (source is GpuDynamicIndicators gpuIndicators)
        {
            return context.Mapper.Map<GpuDynamicHardwareIndicators>(gpuIndicators);
        }
        else if (source is NetworkAdapterDynamicIndicators networkAdapterIndicators)
        {
            return context.Mapper.Map<NetworkAdapterDynamicHardwareIndicators>(networkAdapterIndicators);
        }

        return null!;
    }

    /// <summary>
    /// Конвертировать динамические показатели устройств в динамические показатели майнинга устройств.
    /// </summary>
    /// <param name="source"> Динамические показатели устройств. </param>
    /// <param name="destination"> Динамические показатели майнинга устройств. </param>
    /// <param name="context"> Контекст. </param>
    /// <returns> Динамические показатели майнинга устройств. </returns>
    public IDeviceDynamicMiningIndicators Convert(IDeviceDynamicIndicators source,
                                                  IDeviceDynamicMiningIndicators destination,
                                                  ResolutionContext context)
    {
        if (source is CpuDynamicIndicators cpuIndicators)
        {
            return context.Mapper.Map<CpuDynamicMiningIndicators>(cpuIndicators);
        }
        else if (source is GpuDynamicIndicators gpuIndicators)
        {
            return context.Mapper.Map<GpuDynamicMiningIndicators>(gpuIndicators);
        }

        return null!;
    }
}

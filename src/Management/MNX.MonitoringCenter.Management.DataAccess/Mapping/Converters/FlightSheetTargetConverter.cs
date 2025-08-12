using AutoMapper;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto.Target;

namespace MNX.MonitoringCenter.Management.DataAccess.Mapping.Converters;

/// <summary>
/// Полиморфный конвертер из модели <see cref="FlightSheetTarget"/> в <see cref="BaseFlightSheetTargetDto"/>.
/// </summary>
public class FlightSheetTargetConverter : ITypeConverter<FlightSheetTarget, BaseFlightSheetTargetDto>
{
    ///
    public BaseFlightSheetTargetDto Convert(FlightSheetTarget source, BaseFlightSheetTargetDto destination, ResolutionContext context)
    {
        if (source.DeviceType == MiningDeviceType.CPU)
        {
            return context.Mapper.Map<CpuFlightSheetTargetDto>(source);
        }
        else if (source.DeviceType == MiningDeviceType.GPU)
        {
            return context.Mapper.Map<GpuFlightSheetTargetDto>(source);
        }

        throw new NotSupportedException("Mining device type is not supported!");
    }
}

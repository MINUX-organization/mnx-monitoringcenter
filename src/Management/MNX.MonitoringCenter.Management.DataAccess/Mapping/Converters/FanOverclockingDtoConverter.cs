using AutoMapper;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.DataAccess.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.DataAccess.Mapping.Converters;

/// <summary>
/// Конвертер профиля маппинга для типа <see cref="IFanOverclocking"/>.
/// </summary>
public class FanOverclockingDtoConverter : ITypeConverter<FanOverclockingDto, IFanOverclocking>
{
    ///
    public IFanOverclocking Convert(FanOverclockingDto source, IFanOverclocking destination, ResolutionContext context)
    {
        return source.Type switch
        {
            FanOverclockingType.TargetSpeed => context.Mapper.Map<FanOverclockingWithTargetSpeed>(source),
            FanOverclockingType.TargetTemperature => context.Mapper.Map<FanOverclockingWithTargetTemperature>(source),
            FanOverclockingType.LinearDependence => context.Mapper.Map<FanOverclockingWithLinearDependence>(source),
            _ => throw new NotSupportedException($"{nameof(source.Type)} of fan overclocking does not support")
        };
    }
}

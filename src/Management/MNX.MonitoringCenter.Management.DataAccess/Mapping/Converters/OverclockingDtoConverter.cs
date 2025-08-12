using AutoMapper;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.DataAccess.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;

namespace MNX.MonitoringCenter.Management.DataAccess.Mapping.Converters;

using AmdGpuOverclockingCore = Core.Overclocking.Gpu.AmdGpuOverclocking;

using NvidiaGpuOverclockingCore = Core.Overclocking.Gpu.NvidiaGpuOverclocking;

using IntelGpuOverclockingCore = Core.Overclocking.Gpu.IntelGpuOverclocking;

using CpuOverclockingCore = CpuOverclocking;

/// <summary>
/// Полиморфный конвертер из модели <see cref="OverclockingDto"/> в <see cref="IOverclocking"/>.
/// </summary>
public class OverclockingDtoConverter : ITypeConverter<OverclockingDto, IOverclocking>
{
    ///
    public IOverclocking Convert(OverclockingDto source, IOverclocking destination, ResolutionContext context)
    {
        return source.TargetDeviceType switch
        {
            OverclockingTargetDeviceType.CPU => context.Mapper.Map<CpuOverclockingCore>(source),
            OverclockingTargetDeviceType.AmdGPU => context.Mapper.Map<AmdGpuOverclockingCore>(source),
            OverclockingTargetDeviceType.NvidiaGPU => context.Mapper.Map<NvidiaGpuOverclockingCore>(source),
            OverclockingTargetDeviceType.IntelGPU => context.Mapper.Map<IntelGpuOverclockingCore>(source),
            _ => throw new NotSupportedException("Target device type is not supported!")
        };
    }
}

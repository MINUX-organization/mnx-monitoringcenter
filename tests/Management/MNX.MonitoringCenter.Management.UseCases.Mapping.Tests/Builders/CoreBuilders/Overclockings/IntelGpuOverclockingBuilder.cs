using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.CoreBuilders.Overclockings;

public class IntelGpuOverclockingBuilder :
    BaseOverclockingBuilder<IntelGpuOverclockingBuilder, IntelGpuOverclocking>
{
    public override IOverclocking Build()
    {
        return new IntelGpuOverclocking
        {
            Id = _id,
        };
    }
}

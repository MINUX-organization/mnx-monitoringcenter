using MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.FanOverclocking.Agent;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Agent;

/// <summary>
/// Реализация <see cref="IOverclockingToFanOverclockingAgentMapper{TEntity, TModel}"/>.
/// </summary>
/// <КОСТЫЛИЩЕ>
/// Из-за использования агентом контрактов разгона из инвентаризации,
/// создание сущностей разгона в контрактах Management.Agent.Commands приведет к дублированию контрактов.
/// Выпилить разгон из инвентаризации нельзя по причине того, что в ограничениях Default-значения являются nullable,
/// что ведет к неопредиленности при назначении разгона девайсу.
/// </КОСТЫЛИЩЕ>
public class OverclockingToFanOverclockingAgentMapper
    : IOverclockingToFanOverclockingAgentMapper<IOverclocking, FanOverclocking>
{
    private readonly FanOverclockingAgentMapperRegistry _registry;

    ///
    public OverclockingToFanOverclockingAgentMapper(FanOverclockingAgentMapperRegistry registry)
    {
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
    }

    /// <inheritdoc/>
    public FanOverclocking MapToFanOverclockingModel(IOverclocking entity)
    {
        if (entity is AmdGpuOverclocking amd)
        {
            return _registry.MapToFanOverclockingModel(amd.FanOverclocking);
        }
        if (entity is NvidiaGpuOverclocking nvidia)
        {
            return _registry.MapToFanOverclockingModel(nvidia.FanOverclocking);
        }

        throw new NotSupportedException("Device type does not support");
    }
}
